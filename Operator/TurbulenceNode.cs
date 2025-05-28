using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Turbulence")]
    public class TurbulenceNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.InheritedAny)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Power;

        public override object Run()
        {
            return new Turbulence(
                GetInputValue<double>("Power", this.Power),
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}