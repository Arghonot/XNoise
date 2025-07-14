using UnityEngine;
using XNoise;

namespace Xnoise
{
    public class GPUSurfaceNoiseExecutor : NoiseExecutor
    {
        public override void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
        {
            throw new System.NotImplementedException();
        }

        public override void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true)
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateSpherical(double south, double north, double west, double east)
        {
            throw new System.NotImplementedException();
        }

        public override Texture2D GetNormalMap(float intensity)
        {
            throw new System.NotImplementedException();
        }

        public override Texture2D GetTexture()
        {
            throw new System.NotImplementedException();
        }

        public override Texture2D GetTexture(Gradient gradient)
        {
            throw new System.NotImplementedException();
        }
    }
}