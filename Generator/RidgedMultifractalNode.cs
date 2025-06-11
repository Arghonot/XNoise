using LibNoise;
using LibNoise.Generator;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/RidgedMultifractal")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class RidgedMultifractalNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double lacunarity;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Octaves;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public QualityMode Quality;

        public override object Run()
        {
            //// if editing the graph -> we stick to current variables
            //if (Application.isEditor && !Application.isPlaying)
            //{
            //    return new RidgedMultifractal(
            //        this.frequency,
            //        this.lacunarity,
            //        this.Octaves,
            //        this.Seed,
            //        (QualityMode)this.Quality);
            //}

            return new RidgedMultifractal(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("lacunarity", this.lacunarity),
                GetInputValue<int>("Octaves", this.Octaves),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<QualityMode>("frequency", (QualityMode)this.Quality));
        }
    }
}