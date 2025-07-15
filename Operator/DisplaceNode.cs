using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Displace")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class DisplaceNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Source;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase ControllerA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase ControllerB;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase ControllerC;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Influence = 1;

        public override object Run()
        {
            return new DisplaceCombiner(
                GetInputValue<ModuleBase>("Source", this.Source),
                GetInputValue<ModuleBase>("ControllerA", this.ControllerA),
                GetInputValue<ModuleBase>("ControllerB", this.ControllerB),
                GetInputValue<ModuleBase>("ControllerC", this.ControllerC));
        }
    }
}