using UnityEngine;
using LibNoise.Generator;
using CustomGraph;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Const")]
    [NodeTint(ColorProfile.Input)]
    public class ConstNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double value;

        public override object Run()
        {
            return new ConstGenerator(GetInputValue<double>("value", this.value));
        }
    }
}