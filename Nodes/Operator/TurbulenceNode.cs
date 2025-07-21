using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Turbulence")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class TurbulenceNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.InheritedAny)]
        public ModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Power;

        public override object Run()
        {
            return new TurbulenceTransformer(GetInputValue<double>("Power", this.Power), GetInputValue<ModuleBase>("Input", this.Input));
        }
    }
}