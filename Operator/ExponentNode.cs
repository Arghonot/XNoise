using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Exponent")]
    public class ExponentNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double exponent;

        public override object Run()
        {
            var exp = new Exponent(GetInputValue<SerializableModuleBase>("Input", this.Input));

            exp.Value = GetInputValue<double>("exponent", this.exponent);
            return exp;
        }
    }
}