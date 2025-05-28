using LibNoise;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Debug/ImageReader")]
    public class ImageReaderNode : LibnoiseNode
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