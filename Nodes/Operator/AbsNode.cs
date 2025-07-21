using LibNoise.Operator;
using UnityEngine;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Abs")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class AbsNode : XNoiseNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Controller;

        public override object Run()
        {
            return new AbsCombiner(GetInputValue<ModuleBase>("Controller", this.Controller));
        }
    }
}