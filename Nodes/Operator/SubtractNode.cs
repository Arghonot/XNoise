using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Subtract")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class SubtractNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            return new SubtractCombiner(GetInputValue<ModuleBase>("SourceA", this.SourceA), GetInputValue<ModuleBase>("SourceB", this.SourceB));
        }
    }
}