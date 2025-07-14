using LibNoise;

using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Binarize")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class BinarizeModuleNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;

        public override object Run()
        {
            return new BinarizeModule(GetInputValue<ModuleBase>("Input", this.Input));
        }
    }
}