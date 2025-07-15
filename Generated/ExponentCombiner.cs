using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class ExponentCombiner : LibNoise.Operator.Exponent, INoiseStrategy
    {
        public ExponentCombiner(ModuleBase input) : base(input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Exponent);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetFloat("_Exponent", (float)Value);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}