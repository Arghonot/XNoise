using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class ConstGenerator : LibNoise.Generator.Const, INoiseStrategy
    {
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            // TODO: Implement GPU version
            return null;
        }
    }
}