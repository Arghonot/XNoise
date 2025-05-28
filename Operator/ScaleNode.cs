using UnityEngine;
using LibNoise.Operator;
using LibNoise;
using static XNode.Node;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Scale")]
    public class ScaleNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.InheritedAny)]
        public SerializableModuleBase input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double x;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double y;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double z;

        public override object Run()
        {
            return new Scale(GetInputValue("x", this.x), GetInputValue("y", this.y), GetInputValue("z", this.z), GetInputValue("input", this.input));
        }
    }
}