using LibNoise;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class BillowGenerator : LibNoise.Generator.Billow, INoiseStrategy
    {
        public BillowGenerator(double frequency, double lacunarity, double persistence, int octaves, int seed,
    QualityMode quality) : base(frequency, lacunarity, persistence, octaves, seed, quality) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Billow);

            materialGPU.SetFloat("_Frequency", (float)Frequency);
            materialGPU.SetFloat("_Lacunarity", (float)Lacunarity);
            materialGPU.SetFloat("_Persistence", (float)Persistence);
            materialGPU.SetFloat("_Octaves", OctaveCount);
            materialGPU.SetFloat("_Seed", (float)Seed);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}