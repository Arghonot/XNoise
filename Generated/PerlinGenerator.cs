using UnityEngine;

namespace XNoise
{
    public class PerlinGenerator : LibNoise.Generator.Perlin, INoiseStrategy
    {
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Perlin);
            
            materialGPU.SetFloat("_Frequency", (float)Frequency);
            materialGPU.SetFloat("_Lacunarity", (float)Lacunarity);
            materialGPU.SetFloat("_Persistence", (float)Persistence);
            materialGPU.SetFloat("_Octaves", OctaveCount);
            materialGPU.SetFloat("_Seed", Seed);

            return null; // todo Uncomment the following line when GPUSurfaceNoise2d is available
            //return GPUSurfaceNoise2d.GetImage(_materialGPU, datas, true);
        }
    }
}