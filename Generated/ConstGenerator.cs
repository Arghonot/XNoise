using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class ConstGenerator : LibNoise.Generator.Const, INoiseStrategy
    {
        public ConstGenerator(double value) : base(value) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Const);
            materialGPU.SetFloat("_Const", (float)Value);
            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}