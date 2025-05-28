using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Exponent")]
    public class ExponentNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
            return new Exponent(
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}