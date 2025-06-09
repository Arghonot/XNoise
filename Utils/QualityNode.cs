namespace Xnoise
{

    [CreateNodeMenu("NoiseGraph/Constants/Quality")]
    public class QualityNode : CustomGraph.Leaf<LibNoise.QualityMode>
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)]
        public LibNoise.QualityMode Quality;
    }
}
