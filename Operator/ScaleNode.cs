using UnityEngine;
using LibNoise.Operator;
using LibNoise;
using static XNode.Node;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Scale")]
    public class ScaleNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double X;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Y;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double Z;

        public override object Run()
        {
            return new Scale(GetInputValue<double>("X", this.X),
                GetInputValue<double>("Y", this.Y),
                GetInputValue<double>("Z", this.Z),
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}