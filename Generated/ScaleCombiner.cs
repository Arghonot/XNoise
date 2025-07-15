using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class ScaleCombiner : LibNoise.Operator.Scale, INoiseStrategy
    {
        public ScaleCombiner(double x, double y, double z, ModuleBase input) : base(input) { }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            Vector3 tmpScale = datas.scale;

            datas.scale = new Vector3(datas.scale.x + (float)X, datas.scale.y + (float)Y, datas.scale.z + (float)Z);
            var input = ((INoiseStrategy)Modules[0]).GetValueGPU(datas);
            datas.scale = tmpScale;

            return input;
        }
    }
}