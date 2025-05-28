using LibNoise;
using LibNoise.Generator;

namespace Xnoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Checker")]
    public class CheckerNode : LibnoiseNode
    {
        public override object Run()
        {
            return new Checker();
        }
    }
}