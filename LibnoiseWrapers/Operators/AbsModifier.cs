using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class AbsModifier : LibNoise.Operator.Abs, INoiseStrategy
    {
        public AbsModifier(ModuleBase input) : base(input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Abs);
            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }
    }
}