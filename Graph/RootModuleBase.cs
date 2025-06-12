using System;
using CustomGraph;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [Serializable]
    [HideFromNodeMenu]
    [NodeTint(XNoiseNodeColors.Output)]
    public class RootModuleBase : Root
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
           return GetInputValue<SerializableModuleBase>("Input", this.Input);
        }
    }
}