using LibNoise;
using Graph;

namespace Xnoise
{
    /// <summary>
    /// A default class with a generic output node.
    /// </summary>
    [HideFromNodeMenu]
    public class LibnoiseNode : Graph.Branch<SerializableModuleBase>
    {
        public override object Run()
        {
            return null;
        }
    }
}