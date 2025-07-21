using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Selector/Blend")]
    [NodeTint(XNoiseNodeColors.Selector)]
    public class BlendNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Controller;

        public override object Run()
        {
            return new BlendSelector(
                GetInputValue<ModuleBase>("SourceA", this.SourceA),
                GetInputValue<ModuleBase>("SourceB", this.SourceB),
                GetInputValue<ModuleBase>("Controller", this.Controller));
        }
    }
}