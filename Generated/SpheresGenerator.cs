using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class SpheresGenerator : LibNoise.Generator.Spheres, INoiseStrategy
    {
        public SpheresGenerator(double frequency) : base(frequency) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Spheres);
            materialGPU.SetFloat("_Frequency", (float)Frequency);
            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}