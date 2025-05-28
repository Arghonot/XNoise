using UnityEngine;
using LibNoise;
using LibNoise.Generator;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Cylinders")]
    public class CylindersNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency;

        public override object Run()
        {
            // if editing the graph -> we stick to current variables
            if (Application.isEditor && !Application.isPlaying)
            {
                return new Cylinders(
                    this.frequency);
            }

            return new Cylinders(
                GetInputValue<double>("frequency", this.frequency));
        }
    }
}