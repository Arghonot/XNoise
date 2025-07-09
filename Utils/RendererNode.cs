using CustomGraph;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Debug/Render")]
    [NodeTint(CustomGraph.ColorProfile.Debug)]
    public class RendererNode : Root<SerializableModuleBase>
    {
       [SerializeField] public Renderer renderer = new Renderer();
    }
}

