using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class CheckerGenerator : LibNoise.Generator.Checker, INoiseStrategy
    {
        public CheckerGenerator() : base() { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Checker);
            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}