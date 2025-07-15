using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class MinCombiner : LibNoise.Operator.Min, INoiseStrategy
    {
        public MinCombiner(ModuleBase lhs, ModuleBase rhs) : base(lhs, rhs) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Min);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureB", ((INoiseStrategy)Modules[1]).GetValueGPU(datas));

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}