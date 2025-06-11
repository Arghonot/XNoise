using LibNoise.Operator;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [System.Serializable]
    [CreateNodeMenu("NoiseGraph/Modifier/Curve")]
    [NodeTint(XNoiseNodeColors.Modifier)]
    public class CurveNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase Input;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public AnimationCurve InputCurve;

        public Texture2D animCurve;
        public Texture2D InputTexture;
        public Texture2D RenderedTexture;

        public override object Run()
        {
            Curve curve = new Curve(GetInputValue<SerializableModuleBase>("Input", this.Input));

            curve.mathematicalCurve = GetInputValue<AnimationCurve>("InputCurve", this.InputCurve);

            return curve;
        }
    }
}