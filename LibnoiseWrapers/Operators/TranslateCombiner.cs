using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class TranslateCombiner : LibNoise.Operator.Translate, INoiseStrategy
    {
        public TranslateCombiner(double x, double y, double z, ModuleBase input) : base(x, y, z, input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            Vector3 tmpOrigin = datas.origin;
            datas.origin = new Vector3(datas.origin.x + (float)X, datas.origin.y + (float)Y, datas.origin.z + (float)Z);
            var input = ((INoiseStrategy)Modules[0]).GetValueGPU(datas);
            datas.origin = tmpOrigin;
            return input;
        }
    }
}