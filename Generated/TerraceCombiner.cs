using UnityEngine;
using LibNoise;

namespace XNoise
{
    public class TerraceCombiner : LibNoise.Operator.Terrace, INoiseStrategy
    {
        public TerraceCombiner(bool inverted, ModuleBase input) : base(inverted, input) { }
        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            //_materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Terrace);
            //curve = CreateCircularTerraceCurve(datas, _inverted);
            //var input = ((INoiseStrategy)Modules[0]).GetValueGPU(renderingDatas);

            //_materialGPU.SetTexture("_Src", input);
            //_materialGPU.SetTexture("_Gradient", UtilsFunctions.GetCurveAsTexture(curve));

            //ImageFileHelpers.SaveToJPG(ImageFileHelpers.toTexture2D(input), "/", "TERRACE_INPUT");

            //var res = GPUSurfaceNoise2d.GetImage(_materialGPU, renderingDatas);
            //ImageFileHelpers.SaveToJPG(ImageFileHelpers.toTexture2D(res), "/", "TERRACE_OUTPUT");

            //return res;

            // todo finish me 
            return null;
        }
    }
}