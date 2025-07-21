using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class InvertModifier : LibNoise.Operator.Invert, INoiseStrategy
    {
        public InvertModifier(ModuleBase input) :base(input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Invert);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}