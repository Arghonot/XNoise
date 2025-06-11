using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Rotate")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class RotateNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double XDegrees;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double YDegrees;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double ZDegrees;

        public override object Run()
        {
            return new Rotate(GetInputValue<double>("XDegrees", this.XDegrees),
                GetInputValue<double>("YDegrees", this.YDegrees),
                GetInputValue<double>("ZDegrees", this.ZDegrees),
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}