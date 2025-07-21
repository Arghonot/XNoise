using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Spheres")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class SpheresNode : XNoiseNode
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)]
        public double frequency;

        public override object Run()
        {
            // if editing the graph -> we stick to current variables
            if (Application.isEditor && !Application.isPlaying)
            {
                return new SpheresGenerator(this.frequency);
            }

            return new SpheresGenerator(GetInputValue<double>("frequency", this.frequency));
        }
    }
}