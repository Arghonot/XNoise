using LibNoise;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class PerlinGenerator : LibNoise.Generator.Perlin, INoiseStrategy
    {
        public PerlinGenerator(double frequency, double lacunarity, double persistence, int octaves, int seed,
            QualityMode quality) : base(frequency, lacunarity, persistence, octaves, seed, quality) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Perlin);
            
            materialGPU.SetFloat("_Frequency", (float)Frequency);
            materialGPU.SetFloat("_Lacunarity", (float)Lacunarity);
            materialGPU.SetFloat("_Persistence", (float)Persistence);
            materialGPU.SetFloat("_Octaves", OctaveCount);
            materialGPU.SetFloat("_Seed", Seed);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}