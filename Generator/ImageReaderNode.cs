using CustomGraph;
using UnityEngine;
using XNode;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Debug/ImageReader")]
    [HideFromNodeMenu] // TODO finish me for V2
    public class ImageReaderNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public Texture2D input;
        [Output(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)]
        public Texture2D outputTexture2D;

        public override object GetValue(NodePort port)
        {
            if (port.ValueType == typeof(Texture2D))
            {
                return GetInputValue<Texture2D>("input", this.input);
            }

            return Run();
        }

        public override object Run()
        {
            LibNoise.Image img = new LibNoise.Image(0);
            img.input = GetInputValue<Texture2D>("input", this.input);

            return img;
        }
    }
}