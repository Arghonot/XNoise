using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Exponent")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class ExponentNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double exponent;

        public override object Run()
        {
            var exp = new Exponent(GetInputValue<ModuleBase>("Input", this.Input));

            exp.Value = GetInputValue<double>("exponent", this.exponent);
            return exp;
        }
    }
}