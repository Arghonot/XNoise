using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class SelectCombiner : LibNoise.Operator.Select, INoiseStrategy
    {
        public SelectCombiner(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(inputA, inputB, controller) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Select);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureB", ((INoiseStrategy)Modules[1]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureC", ((INoiseStrategy)Modules[2]).GetValueGPU(datas));
            materialGPU.SetFloat("_FallOff", (float)FallOff);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}