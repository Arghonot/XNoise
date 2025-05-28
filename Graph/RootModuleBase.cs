using System;
using LibNoise;
using UnityEngine;

namespace Xnoise
{
    [Serializable]
    public class RootModuleBase : Graph.Root//<SerializableModuleBase>
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        public override object Run()
        {
           return GetInputValue<SerializableModuleBase>("Input", this.Input);
        }
    }
}