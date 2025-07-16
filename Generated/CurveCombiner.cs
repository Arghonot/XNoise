using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class CurveCombiner : LibNoise.Operator.Curve, INoiseStrategy
    {
        private Texture2D curve;
        public AnimationCurve mathematicalCurve;

        public CurveCombiner(ModuleBase input): base(input) { }
        public CurveCombiner(ModuleBase input, AnimationCurve curveInput) : base(input)
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

    }
}