using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Billow")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class BillowNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency = 0.6;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double lacunarity = 3;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double persistence = 0.4;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Octaves = 6;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public QualityMode Quality;

        public override object Run()
        {
            return new BillowGenerator(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("lacunarity", this.lacunarity),
                GetInputValue<double>("persistence", this.persistence),
                GetInputValue<int>("Octaves", this.Octaves),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<QualityMode>("Quality", this.Quality));
        }
    }
}