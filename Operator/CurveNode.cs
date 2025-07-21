using LibNoise;
using UnityEngine;

namespace XNoise
{
    [System.Serializable]
    [CreateNodeMenu("NoiseGraph/Modifier/Curve")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class CurveNode : XNoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase Input;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public AnimationCurve InputCurve;

        protected override void Init()
        {
            base.Init();
            InputCurve = CurveCombiner.CreateLinearCurve();
        }

        public override object Run()
        {
           return new CurveCombiner(GetInputValue<ModuleBase>("Input", this.Input), GetInputValue<AnimationCurve>("InputCurve", this.InputCurve));
        }
    }
}