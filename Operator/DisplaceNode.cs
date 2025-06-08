using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Transformer/Displace")]
    public class DisplaceNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Source;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase ControllerA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase ControllerB;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase ControllerC;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Influence = 1;

        public override object Run()
        {
            return new Displace(
                GetInputValue<SerializableModuleBase>("Source", this.Source),
                GetInputValue<SerializableModuleBase>("ControllerA", this.ControllerA),
                GetInputValue<SerializableModuleBase>("ControllerB", this.ControllerB),
                GetInputValue<SerializableModuleBase>("ControllerC", this.ControllerC),
                GetInputValue<double>("Influence", this.Influence));
        }
    }
}