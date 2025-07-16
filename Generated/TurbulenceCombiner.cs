using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class TurbulenceCombiner : LibNoise.Operator.Turbulence, INoiseStrategy
    {
        #region Protected Constants

        protected const double X0 = (12414.0 / 65536.0);
        protected const double Y0 = (65124.0 / 65536.0);
        protected const double Z0 = (31337.0 / 65536.0);
        protected const double X1 = (26519.0 / 65536.0);
        protected const double Y1 = (18128.0 / 65536.0);
        protected const double Z1 = (60493.0 / 65536.0);
        protected const double X2 = (53820.0 / 65536.0);
        protected const double Y2 = (11213.0 / 65536.0);
        protected const double Z2 = (44845.0 / 65536.0);

        #endregion

        public TurbulenceCombiner(double power, ModuleBase input) : base(power, input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Turbulence);

            var tmpTurbulence = datas.displacementMap;
            Vector3 tmpOrigin = datas.origin;

            materialGPU.SetTexture("_OriginalDisplacementMap", datas.displacementMap);
            datas.origin = new Vector3((float)X0, (float)Y0, (float)Z0);
            materialGPU.SetTexture("_PerlinA", new PerlinGenerator().GetValueGPU(datas));
            datas.origin = new Vector3((float)X1, (float)Y1, (float)Z1);
            materialGPU.SetTexture("_PerlinB", new PerlinGenerator().GetValueGPU(datas));
            datas.origin = new Vector3((float)X2, (float)Y2, (float)Z2);
            materialGPU.SetTexture("_PerlinC", new PerlinGenerator().GetValueGPU(datas));
            materialGPU.SetFloat("_Power", (float)Power);
            datas.origin = tmpOrigin;

            datas.displacementMap = GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);

            return ((INoiseStrategy)Modules[0]).GetValueGPU(datas);

        }
    }
}