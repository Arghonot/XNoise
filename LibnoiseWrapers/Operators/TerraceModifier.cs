using UnityEngine;
using LibNoise;
using Xnoise;
using System.Collections.Generic;

namespace XNoise
{
    public class TerraceModifier : LibNoise.Operator.Terrace, INoiseStrategy
    {
        public AnimationCurve curve;

        public TerraceModifier(bool inverted, ModuleBase input) : base(inverted, input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Terrace);

            curve = CreateCircularTerraceCurve(ControlPoints, IsInverted);
            var input = ((INoiseStrategy)Modules[0]).GetValueGPU(datas);
            materialGPU.SetTexture("_Src", input);
            materialGPU.SetTexture("_Gradient", UtilsFunctions.GetCurveAsTexture(curve));

            return GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
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

                var k0 = new Keyframe(x0, y0, 0, 0);
                curve.AddKey(k0);

                float xm = x0 + dx / 2f;
                float ym = (y0 + y1) / 2f;

                float arcSlope = (y1 - y0) / (dx * 0.5f); // Tangent as rise/run
                var km = new Keyframe(xm, ym, arcSlope, arcSlope);
                curve.AddKey(km);

                var k1 = new Keyframe(x1, y1, 0, 0);
                curve.AddKey(k1);
            }

            return curve;
        }
    }
}