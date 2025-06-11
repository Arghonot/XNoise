using UnityEngine;
using LibNoise.Operator;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Min")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class MinNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceB;

        public override object Run()
        {
            Min min = new Min(
                GetInputValue<SerializableModuleBase>("SourceA", this.SourceA),
                GetInputValue<SerializableModuleBase>("SourceB", this.SourceB));

            return min;
        }
    }
}