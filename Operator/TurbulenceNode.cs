using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Turbulence")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class TurbulenceNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.InheritedAny)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Power;

        public override object Run()
        {
            return new Turbulence(
                GetInputValue<double>("Power", this.Power),
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}