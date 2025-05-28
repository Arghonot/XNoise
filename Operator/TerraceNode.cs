using LibNoise.Operator;
using LibNoise;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Terrace")]
    public class TerraceNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase InputModule;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict, true)]
        public double[] controlPoints;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public bool inverted;

        public Material mat;

        public override object Run()
        {
            if (controlPoints.Length == 0)
            {
                return GetInputValue<SerializableModuleBase>("InputModule", this.InputModule);
            }

            Terrace terrace = new Terrace(inverted, GetInputValue<SerializableModuleBase>("InputModule", this.InputModule));

            for (int i = 0; i < controlPoints.Length; i++)
            {
                terrace.Add(GetInputValue(i));
            }

            mat = terrace._materialGPU;

            return terrace;
        }

        private double GetInputValue(int index)
        {
            if (!GetPort("controlPoints " + index.ToString()).IsConnected)
            {
                return controlPoints[index];
            }
            else
            {
                return GetPort("controlPoints " + index.ToString()).GetInputValue<double>();
            }
        }
    }
}