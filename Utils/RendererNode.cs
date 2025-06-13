using CustomGraph;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Debug/Render")]
    [NodeTint(CustomGraph.ColorProfile.Debug)]
    public class RendererNode : Root
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        [SerializeField] public Renderer renderer = new Renderer();

        public override object Run()
        {
            return GetInputValue<SerializableModuleBase>("Input", this.Input);
        }
    }
}

