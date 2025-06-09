using System;
using Graph;
using LibNoise;
using UnityEngine;

namespace Xnoise
{
    [Serializable]
    [HideFromNodeMenu]
    public class RootModuleBase : Graph.Root
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
           return GetInputValue<SerializableModuleBase>("Input", this.Input);
        }
    }
}