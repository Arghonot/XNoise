using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class CylindersGenerator : LibNoise.Generator.Cylinders, INoiseStrategy
    {
        public CylindersGenerator(double frequency) : base(frequency) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Cylinders);
            materialGPU.SetFloat("_Frequency", (float)Frequency);
            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas, true);
        }
    }
}