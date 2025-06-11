using LibNoise.Generator;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Voronoi")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class VoronoiNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double displacement;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public bool distance;

        public override object Run()
        {
            return new Voronoi(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("displacement", this.displacement),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<bool>("distance", this.distance));
        }
    }
}