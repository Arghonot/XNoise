using UnityEngine;
using LibNoise.Operator;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Power")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class PowerNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceB;

        public override object Run()
        {
            Power power = new Power(
                GetInputValue<SerializableModuleBase>("SourceA", this.SourceA),
                GetInputValue<SerializableModuleBase>("SourceB", this.SourceB));

            return power;
        }
    }
}