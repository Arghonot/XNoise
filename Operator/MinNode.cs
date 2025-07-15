using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Min")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class MinNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            return new MinCombiner(
                GetInputValue<ModuleBase>("SourceA", this.SourceA),
                GetInputValue<ModuleBase>("SourceB", this.SourceB));
        }
    }
}