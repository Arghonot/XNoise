using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class ScaleBiasModifier : LibNoise.Operator.ScaleBias, INoiseStrategy
    {
        public ScaleBiasModifier(ModuleBase input) : base(input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.ScaleBias);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetFloat("_Bias", (float)Bias);
            materialGPU.SetFloat("_Scale", (float)Scale);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}