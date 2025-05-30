﻿using LibNoise;
using LibNoise.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Voronoi")]
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
            //// if editing the graph -> we stick to current variables
            //if (Application.isEditor && !Application.isPlaying)
            //{
            //    return new Voronoi(
            //        this.frequency,
            //        this.displacement,
            //        this.Seed,
            //        this.distance);
            //}

            //return new Voronoi(frequency, displacement, Seed, distance);
            return new Voronoi(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("displacement", this.displacement),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<bool>("distance", this.distance));
        }
    }
}