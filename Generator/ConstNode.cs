using UnityEngine;
using LibNoise.Generator;
using CustomGraph;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Const")]
    [NodeTint(ColorProfile.Input)]
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