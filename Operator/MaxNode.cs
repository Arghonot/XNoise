using UnityEngine;
using LibNoise.Operator;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Max")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class MaxNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            return new MaxCombiner(GetInputValue<ModuleBase>("SourceA", this.SourceA), GetInputValue<ModuleBase>("SourceB", this.SourceB));
        }
    }
}