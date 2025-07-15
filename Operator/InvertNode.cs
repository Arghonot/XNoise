using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Invert")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class InvertNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;

        public override object Run()
        {
            return new InvertCombiner(GetInputValue<ModuleBase>("Input", this.Input));

        }
    }
}