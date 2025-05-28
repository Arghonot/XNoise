using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using static XNode.Node;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Const")]
    public class ConstNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double value;

        public override object Run()
        {
            //if editing the graph -> we stick to current variables
            if (Application.isEditor && !Application.isPlaying)
            {
                return new Const(this.value);
            }

            return new Const(
                GetInputValue<double>("value", this.value));
        }
    }
}