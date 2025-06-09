using Graph;
using LibNoise;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Graph/LibNoiseSubGraph")]
    [HideFromNodeMenu] // TODO finish me for V2
    public class LibNoiseSubGraph : Graph.SubGraphNode<SerializableModuleBase>
    {
        public override object Run()
        {
            if (targetSubGraph == null)
            {
                return new SerializableModuleBase(0);
            }

            return ((XnoiseGraph)targetSubGraph).Run(GenerateProperStorage());
        }
    }
}