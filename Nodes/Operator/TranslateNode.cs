using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Translate")]
    [NodeTint(XNoiseNodeColors.Transformer)]
    public class TranslateNode : XNoiseNode
    {

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double X;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Y;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Z;

        public override object Run()
        {
            return new TranslateCombiner(
                GetInputValue<double>("X", this.X),
                GetInputValue<double>("Y", this.Y),
                GetInputValue<double>("Z", this.Z),
                GetInputValue<ModuleBase>("Input", this.Input));
        }
    }
}