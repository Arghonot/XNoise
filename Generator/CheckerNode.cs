using LibNoise.Generator;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Checker")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class CheckerNode : LibnoiseNode
    {
        public override object Run()
        {
            return new Checker();
        }
    }
}