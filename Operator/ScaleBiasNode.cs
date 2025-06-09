using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/ScaleBias")]
    public class ScaleBiasNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Bias;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Scale;

        public override object Run()
        {
            ScaleBias bias = new ScaleBias(
                GetInputValue<SerializableModuleBase>("Input", this.Input));

            bias.Scale = GetInputValue<double>("Scale", this.Scale);
            bias.Bias = GetInputValue<double>("Bias", this.Bias);

            return bias;
        }
    }
}