using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Perlin")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class PerlinNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency = 3;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double lacunarity = 2;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double persistence = 0.5;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Octaves = 6;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public QualityMode Quality;

        public override object Run()
        {
            return new PerlinGenerator(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("lacunarity", this.lacunarity),
                GetInputValue<double>("persistence", this.persistence),
                GetInputValue<int>("Octaves", this.Octaves),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<QualityMode>("Quality", this.Quality));
        }
    }
}
