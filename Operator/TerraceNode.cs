using LibNoise.Operator;
using LibNoise;
using UnityEngine;
using System.Security.Cryptography;
using LibNoise.Generator;
using System.Linq;
using System.Collections.Generic;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Modifier/Terrace")]
    public class TerraceNode : LibnoiseNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public SerializableModuleBase InputModule;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict, true)]
        public double[] controlPoints;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public bool inverted;

        public override object Run()
        {
            if (controlPoints.Length == 0)
            {
                return GetInputValue<SerializableModuleBase>("InputModule", this.InputModule);
            }

            Terrace terrace = new Terrace(inverted, GetInputValue<SerializableModuleBase>("InputModule", this.InputModule));

            for (int i = 0; i < controlPoints.Length; i++)
            {
                terrace.Add(GetInputValue(i));
            }

            animCurve = Terrace.GenerateAnimationCurve(controlPoints.ToList());
            return terrace;
        }

        private double GetInputValue(int index)
        {
            if (!GetPort("controlPoints " + index.ToString()).IsConnected)
            {
                return controlPoints[index];
            }
            else
            {
                return GetPort("controlPoints " + index.ToString()).GetInputValue<double>();
            }
        }

        public AnimationCurve animCurve;
        public Texture2D curve;

        [ContextMenu("Debug Texture")]
        private void DebugTexture()
        {
            curve = UtilsFunctions.GetCurveAsTexture(Terrace.GenerateAnimationCurve(controlPoints.ToList()));

            ImageFileHelpers.SaveToJPG(curve, "/", "map");
        }

        [ContextMenu("Debug Curve")]
        private void DebugCurve()
        {
            animCurve = CreateCircularTerraceCurve(controlPoints.ToList());
            curve = UtilsFunctions.GetCurveAsTexture(Terrace.CreateCircularTerraceCurve(controlPoints.ToList()));

            ImageFileHelpers.SaveToJPG(curve, "/", "GeneratedMap");
        }

        public static AnimationCurve CreateTerraceCurve(List<double> ctrlPts, bool invert = false)
        {
            var cps = ctrlPts;
            if (cps == null || cps.Count < 2)
                throw new System.ArgumentException("Need at least 2 control points");

            var curve = new AnimationCurve();

            // Helper for cubic S-curve
            float SCurve(float t) => (float)(-2 * t * t * t + 3 * t * t);

            for (int i = 0; i < cps.Count - 1; i++)
            {
                float x0 = (float)cps[i];
                float x1 = (float)cps[i + 1];
                float dx = x1 - x0;
                if (dx <= 0) continue;

                // Add key at start of plateau
                curve.AddKey(new Keyframe(x0, invert ? x1 : x0, 0, 0));

                // Add interstitial point at t=0.5 with steep slope
                float mid = x0 + dx * 0.5f;
                float midVal = invert ? Mathf.Lerp(x1, x0, SCurve(0.5f)) : Mathf.Lerp(x0, x1, SCurve(0.5f));
                curve.AddKey(new Keyframe(mid, midVal, float.PositiveInfinity, float.PositiveInfinity));

                // Add key at end plateau
                curve.AddKey(new Keyframe(x1, invert ? x0 : x1, 0, 0));
            }

            return curve;
        }

        public static AnimationCurve CreateCircularTerraceCurve(List<double> ctrlPts, bool invert = false)
        {
            if (ctrlPts == null || ctrlPts.Count < 2)
                throw new System.ArgumentException("Need at least 2 control points");

            var curve = new AnimationCurve();

            for (int i = 0; i < ctrlPts.Count - 1; i++)
            {
                float x0 = (float)ctrlPts[i];
                float x1 = (float)ctrlPts[i + 1];
                float dx = x1 - x0;
                if (dx <= 0) continue;

                float y0 = invert ? x1 : x0;
                float y1 = invert ? x0 : x1;

                // Add flat start of terrace
                var k0 = new Keyframe(x0, y0, 0, 0);
                curve.AddKey(k0);

                // Midpoint for arc (halfway between)
                float xm = x0 + dx / 2f;
                float ym = (y0 + y1) / 2f;

                // Approximate semicircle shape with symmetric tangents
                float arcSlope = (y1 - y0) / (dx * 0.5f); // Tangent as rise/run
                var km = new Keyframe(xm, ym, arcSlope, arcSlope);
                curve.AddKey(km);

                // Add flat end of terrace
                var k1 = new Keyframe(x1, y1, 0, 0);
                curve.AddKey(k1);
            }

            return curve;
        }

    }
}