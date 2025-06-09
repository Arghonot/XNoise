using LibNoise;
using CustomGraph;

namespace Xnoise
{
    /// <summary>
    /// A default class with a generic output node.
    /// </summary>
    [HideFromNodeMenu]
    public class LibnoiseNode : CustomGraph.Branch<SerializableModuleBase>
    {
        public override object Run()
        {
            return null;
        }
    }
}