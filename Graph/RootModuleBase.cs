using System;
using CustomGraph;
using LibNoise;

namespace XNoise
{
    [Serializable]
    [HideFromNodeMenu]
    [NodeTint(XNoiseNodeColors.Output)]
    public class RootModuleBase : Root<SerializableModuleBase> { }
}