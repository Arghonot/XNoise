using LibNoise;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class BlendCombiner : LibNoise.Operator.Blend, INoiseStrategy
    {
        public BlendCombiner(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(lhs, rhs, controller) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Blend);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureB", ((INoiseStrategy)Modules[1]).GetValueGPU(datas));
            materialGPU.SetTexture("_Controller", ((INoiseStrategy)Modules[2]).GetValueGPU(datas));

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}