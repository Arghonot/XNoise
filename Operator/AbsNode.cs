using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Abs")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class AbsNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Controller;

        public override object Run()
        {
            return new Abs(GetInputValue<SerializableModuleBase>("Controller", this.Controller));
        }
    }
}