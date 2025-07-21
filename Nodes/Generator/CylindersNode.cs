using UnityEngine;
using LibNoise.Generator;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Cylinders")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class CylindersNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency;

        public override object Run()
        {
            // if editing the graph -> we stick to current variables
            if (Application.isEditor && !Application.isPlaying)
            {
                return new CylindersGenerator(this.frequency);
            }

            return new CylindersGenerator(GetInputValue<double>("frequency", this.frequency));
        }
    }
}