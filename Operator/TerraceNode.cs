using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Terrace")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class TerraceNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase InputModule;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict, true)]
        public double[] controlPoints;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public bool inverted;

        public override object Run()
        {
            if (controlPoints.Length == 0)
            {
                return GetInputValue<ModuleBase>("InputModule", this.InputModule);
            }

            TerraceCombiner terrace = new TerraceCombiner(inverted, GetInputValue<ModuleBase>("InputModule", this.InputModule));

            for (int i = 0; i < controlPoints.Length; i++)
            {
                terrace.Add(GetInputValue(i));
            }

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