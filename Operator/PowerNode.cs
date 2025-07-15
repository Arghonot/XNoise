using UnityEngine;
using LibNoise.Operator;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Power")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class PowerNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            Power power = new PowerCombiner(
                GetInputValue<ModuleBase>("SourceA", this.SourceA),
                GetInputValue<ModuleBase>("SourceB", this.SourceB));

            return power;
        }
    }
}