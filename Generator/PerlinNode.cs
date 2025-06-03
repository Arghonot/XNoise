using LibNoise;
using LibNoise.Generator;
using System.Linq;
using UnityEngine;
using XNode;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Perlin")]
    public class PerlinNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double lacunarity;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double persistence;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Octaves;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public QualityMode Quality;

        [ContextMenu("Debugdkmj")]
        public void Debugdkmj()
        {
            Debug.Log("frequency " + GetInputValue<double>("frequency", this.frequency));
            Debug.Log("lacunarity " + GetInputValue<double>("lacunarity", this.lacunarity));
            Debug.Log("persistence " + GetInputValue<double>("persistence", this.persistence));
            Debug.Log("Octaves " + GetInputValue<int>("Octaves", this.Octaves));
            Debug.Log("Seed " + GetInputValue<int>("Seed", this.Seed));
        }

        public override object Run()
        {
            return new Perlin(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("lacunarity", this.lacunarity),
                GetInputValue<double>("persistence", this.persistence),
                GetInputValue<int>("Octaves", this.Octaves),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<QualityMode>("Quality", this.Quality));
        }
    }
}
