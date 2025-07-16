using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Multiply")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class MultiplyNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            return new MultiplyCombiner(GetInputValue<ModuleBase>("SourceA", this.SourceA), GetInputValue<ModuleBase>("SourceB", this.SourceB));
        }
    }
}