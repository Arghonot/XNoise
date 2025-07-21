using LibNoise;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class RidgedMultifractalGenerator : LibNoise.Generator.RidgedMultifractal, INoiseStrategy
    {
        public RidgedMultifractalGenerator(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(frequency, lacunarity, octaves, seed, quality) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.RidgedMultifractal);

            materialGPU.SetFloat("_Frequency", (float)Frequency);
            materialGPU.SetFloat("_Lacunarity", (float)Lacunarity);
            materialGPU.SetFloat("_Seed", (float)Seed);
            materialGPU.SetFloat("_Octaves", OctaveCount);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}