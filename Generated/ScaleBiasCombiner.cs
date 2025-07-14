using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class ScaleBiasCombiner : LibNoise.Operator.ScaleBias, INoiseStrategy
    {
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            // TODO: Implement GPU version
            return null;
        }
    }
}