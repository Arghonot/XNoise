using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class TurbulenceCombiner : LibNoise.Operator.Turbulence, INoiseStrategy
    {
        public TurbulenceCombiner(double power, ModuleBase input) : base(power, input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            //_materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Turbulence);

            //var tmpTurbulence = renderingDatas.displacementMap;
            //Vector3 tmpOrigin = renderingDatas.origin;
            //_materialGPU.SetTexture("_OriginalDisplacementMap", renderingDatas.displacementMap);
            //renderingDatas.origin = new Vector3((float)X0, (float)Y0, (float)Z0);
            //_materialGPU.SetTexture("_PerlinA", new Perlin().GetValueGPU(renderingDatas));
            //renderingDatas.origin = new Vector3((float)X1, (float)Y1, (float)Z1);
            //_materialGPU.SetTexture("_PerlinB", new Perlin().GetValueGPU(renderingDatas));
            //renderingDatas.origin = new Vector3((float)X2, (float)Y2, (float)Z2);
            //_materialGPU.SetTexture("_PerlinC", new Perlin().GetValueGPU(renderingDatas));
            //_materialGPU.SetFloat("_Power", (float)Power);
            //renderingDatas.origin = tmpOrigin;

            //ImageFileHelpers.SaveToJPG(ImageFileHelpers.toTexture2D(renderingDatas.displacementMap), "/", "TURBULENCE_BEFORE");
            //renderingDatas.displacementMap = GPUSurfaceNoise2d.GetImage(_materialGPU, renderingDatas);
            //ImageFileHelpers.SaveToJPG(ImageFileHelpers.toTexture2D(renderingDatas.displacementMap), "/", "TURBULENCE_AFTER");

            //var value = Modules[0].GetValueGPU(renderingDatas);

            //return value;

            // todo finish me 
            return null;
        }
    }
}