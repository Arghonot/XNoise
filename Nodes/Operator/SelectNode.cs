using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Selector/Select")]
    [NodeTint(XNoiseNodeColors.Selector)]
    public class SelectNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Controller;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public float Falloff;

        public override object Run()
        {
            Select select = new SelectCombiner(
                GetInputValue<ModuleBase>("SourceA", this.SourceA),
                GetInputValue<ModuleBase>("SourceB", this.SourceB),
                GetInputValue<ModuleBase>("Controller", this.Controller));

            select.FallOff = GetInputValue<float>("Falloff", this.Falloff);

            return select;
        }
    }
}