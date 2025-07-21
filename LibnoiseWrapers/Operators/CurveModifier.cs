using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class CurveModifier : LibNoise.Operator.Curve, INoiseStrategy
    {
        private Texture2D curve;
        public AnimationCurve mathematicalCurve;

        public static AnimationCurve CreateLinearCurve() => new AnimationCurve(
            new Keyframe(-1f, -1f, 1f, 1f),
            new Keyframe(1f, 1f, 1f, 1f)
        );

        public CurveModifier(ModuleBase input): base(input)
        {
            mathematicalCurve = CreateLinearCurve();
        }
        public CurveModifier(ModuleBase input, AnimationCurve curveInput) : base(input)
        {
            mathematicalCurve = curveInput;
            SetCurve(UtilsFunctions.GetCurveAsTexture(curveInput));
        }

        public double GetValueCPU(double x, double y, double z)
        {
            Debug.Assert(Modules[0] != null);
            Debug.Assert(ControlPointCount >= 4);
            double val = ((INoiseStrategy)Modules[0]).GetValueCPU(x, y, z);

            return mathematicalCurve.Evaluate((float)val);
        }

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            if (!CoversMinus1To1(mathematicalCurve)) return ((INoiseStrategy)Modules[0]).GetValueGPU(datas);

            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Curve);

            if (curve == null)
            {
                curve = UtilsFunctions.GetCurveAsTexture(mathematicalCurve);
            }
            materialGPU.SetTexture("_Src", ((INoiseStrategy)Modules[0]).GetValueGPU(datas));
            materialGPU.SetTexture("_Gradient", curve);

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
        }

        public void SetCurve(Texture2D newcurve)
        {
            curve = newcurve;
        }

        public static bool CoversMinus1To1(AnimationCurve curve)
        {
            if (curve == null || curve.length < 2) return false;

            // Walk every unordered pair of keys
            for (int i = 0; i < curve.length - 1; i++)
            {
                var a = curve[i];
                for (int j = i + 1; j < curve.length; j++)
                {
                    var b = curve[j];

                    // Sort extremes for clarity
                    float minTime = Mathf.Min(a.time, b.time);
                    float maxTime = Mathf.Max(a.time, b.time);
                    float minValue = Mathf.Min(a.value, b.value);
                    float maxValue = Mathf.Max(a.value, b.value);

                    // If the segment’s bounding box contains the –1→1 square,
                    // then evaluating within that time range is guaranteed
                    // to produce values in –1 -> 1 (or beyond).
                    bool timeOK = minTime <= -1f && maxTime >= 1f;
                    bool valueOK = minValue <= -1f && maxValue >= 1f;

                    if (timeOK && valueOK)
                        return true;
                }
            }
            return false;
        }
    }
}