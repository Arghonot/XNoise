using LibNoise.Operator;
using LibNoise;

using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Binarize")]
    public class BinarizeModuleNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
            return new BinarizeModule(
                GetInputValue<SerializableModuleBase>("Input", this.Input));
        }
    }
}