using UnityEngine;
using LibNoise;
using LibNoise.Operator;

namespace XNoise
{
    public class CurveCombiner : LibNoise.Operator.Curve, INoiseStrategy
    {
        public CurveCombiner(ModuleBase input): base(input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            //var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Curve);

            //if (Curve == null)
            //{
            //    curve = UtilsFunctions.GetCurveAsTexture(mathematicalCurve);
            //}

            //materialGPU.SetTexture("_Src", Modules[0].GetValueGPU(datas));
            //materialGPU.SetTexture("_Gradient", curve);

            //return GPUSurfaceNoise2d.GetImage(materialGPU, datas);
            return null; // todo need full replacement with my update
        }
    }
}