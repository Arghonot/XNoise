using UnityEngine;

namespace XNoise
{
    public interface INoiseStrategy
    {
        public double GetValueCPU(double x, double y, double z);
        public RenderTexture GetValueGPU(GPURenderingDatas datas);
    }
}