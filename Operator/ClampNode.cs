using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Clamp")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class ClampNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Minimum;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Maximum;

        public override object Run()
        {
            Clamp clamp = new ClampCombiner(GetInputValue<ModuleBase>("Input", this.Input));

            clamp.Minimum = GetInputValue<double>("Minimum", this.Minimum);
            clamp.Maximum = GetInputValue<double>("Maximum", this.Maximum);
            return clamp;
        }
    }
}