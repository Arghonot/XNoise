﻿using CustomGraph;
using LibNoise;
using UnityEngine.Android;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Graph/LibNoiseSubGraph")]
    [HideFromNodeMenu] // TODO finish me for V2
    public class LibNoiseSubGraph : SubGraphNode<ModuleBase>
    {
        public override object Run()
        {
            if (targetSubGraph == null)
            {
                // TODO fix me
                //return new ModuleBase(0);
            }

            return ((XnoiseGraph)targetSubGraph).Run(GenerateProperStorage());
        }
    }
}