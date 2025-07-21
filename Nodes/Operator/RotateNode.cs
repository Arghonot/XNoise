using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Rotate")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class RotateNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double XDegrees;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double YDegrees;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double ZDegrees;

        public override object Run()
        {
            return new RotateTransformer(GetInputValue<double>("XDegrees", this.XDegrees),
                GetInputValue<double>("YDegrees", this.YDegrees),
                GetInputValue<double>("ZDegrees", this.ZDegrees),
                GetInputValue<ModuleBase>("Input", this.Input));
        }
    }
}