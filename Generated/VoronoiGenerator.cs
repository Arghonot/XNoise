using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class VoronoiGenerator : LibNoise.Generator.Voronoi, INoiseStrategy
    {
        public VoronoiGenerator(double frequency, double displacement, int seed, bool distance) : base(frequency, displacement, seed, distance) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Voronoi);

            materialGPU.SetFloat("_Displacement", (float)Displacement);
            materialGPU.SetFloat("_Frequency", (float)Frequency);
            materialGPU.SetInt("_Distance", UseDistance ? 2 : 0); // todo could cause problem depending on shader !!
            materialGPU.SetInt("_Seed", Seed);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}