using LibNoise;
using LibNoise.Generator;
using UnityEngine;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Spheres")]
    public class SpheresNode : LibnoiseNode
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)]
        public double frequency;

        public override object Run()
        {
            // if editing the graph -> we stick to current variables
            if (Application.isEditor && !Application.isPlaying)
            {
                return new Spheres(this.frequency);
            }

            return new Spheres(GetInputValue<double>("frequency", this.frequency));
        }
    }
}