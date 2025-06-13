using CustomGraph;

namespace XNoise
{

    [CreateNodeMenu("NoiseGraph/Constants/Quality")]
    [NodeTint(ColorProfile.Input)]
    public class QualityNode : Leaf<LibNoise.QualityMode>
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)]
        public LibNoise.QualityMode Quality;
    }
}
