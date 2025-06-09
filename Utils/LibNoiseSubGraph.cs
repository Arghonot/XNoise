using CustomGraph;
using LibNoise;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Graph/LibNoiseSubGraph")]
    [HideFromNodeMenu] // TODO finish me for V2
    public class LibNoiseSubGraph : CustomGraph.SubGraphNode<SerializableModuleBase>
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