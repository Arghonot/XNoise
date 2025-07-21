using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class RotateCombiner : LibNoise.Operator.Rotate, INoiseStrategy
    {
        public RotateCombiner(double x, double y, double z, ModuleBase input) : base(x, y, z, input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            Vector3 tmpRotation = datas.rotation;

            datas.rotation = new Vector3(datas.rotation.x + (float)X, datas.rotation.y + (float)Y, datas.rotation.z + (float)Z);
            var input = ((INoiseStrategy)Modules[0]).GetValueGPU(datas);
            datas.rotation = tmpRotation;

            return input;
        }
    }
}