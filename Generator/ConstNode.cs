using UnityEngine;
using LibNoise.Generator;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Const")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class ConstNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double value;

        public override object Run()
        {
            return new Const(GetInputValue<double>("value", this.value));
        }
    }
}