using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class ClampModifier : LibNoise.Operator.Clamp, INoiseStrategy
    {
        public ClampModifier(ModuleBase input) : base(input) { }

        public ClampModifier(double min, double max, ModuleBase input): base(min, max, input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Clamp);

            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetFloat("_Minimum", (float)Minimum);
            materialGPU.SetFloat("_Maximum", (float)Maximum);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}