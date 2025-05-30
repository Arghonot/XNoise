﻿using UnityEngine;
using LibNoise.Operator;
using LibNoise;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Max")]
    public class MaxNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceA;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase SourceB;

        public override object Run()
        {
            Max max = new Max(
                GetInputValue<SerializableModuleBase>("SourceA", this.SourceA),
                GetInputValue<SerializableModuleBase>("SourceB", this.SourceB));

            return max;
        }
    }
}