using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Invert")]
    public class InvertNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
            return new Invert(
                GetInputValue<SerializableModuleBase>("Input", this.Input));

        }
    }
}