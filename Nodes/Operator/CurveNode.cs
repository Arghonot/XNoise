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
            InputCurve = CurveModifier.CreateLinearCurve();
        }

        public override object Run()
        {
           return new CurveModifier(GetInputValue<ModuleBase>("Input", this.Input), GetInputValue<AnimationCurve>("InputCurve", this.InputCurve));
        }
    }
}