using UnityEngine;

namespace XNoise
{
    public class CheckerGenerator : LibNoise.Generator.Checker, INoiseStrategy
    {
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            // TODO: Implement GPU version
            return null;
        }
    }
}