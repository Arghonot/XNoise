using LibNoise;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class Renderer
    {
        [HideInInspector] public static int index = 0;
        // TODO move to a nested class for better clarity
        string DataPath = "/";
        [HideInInspector] public string PictureName = "Test";

        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 0;
        [HideInInspector] public RenderTexture rtex = null;
        [HideInInspector] public Texture2D tex = null;
        [HideInInspector] public Gradient grad = new Gradient();

        [HideInInspector] public int renderMode; // todo use enum here
        [HideInInspector] public int renderType; // todo use enum here
        [HideInInspector] public int projectionMode; // todo use enum here

        [HideInInspector] public long RenderTime;

        public float intensity = 100f;

        [HideInInspector] public INoiseStrategy input;

        private NoiseExecutor _noise;

        public Renderer() { }

        public void Render()
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            if (renderMode == 0)
            {
                _noise = new CPUNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
            }
            else if (renderMode == 1)
            {
                _noise = new GPUSurfaceNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
            }
            
            RenderHeightMap();
            if (renderType == 1)
            {
                RenderNormalMap();
            }

            //watch.Stop();
            //RenderTime = watch.ElapsedMilliseconds;
        }

        public void RenderHeightMap()
        {
            if (projectionMode == 0)
            {
                _noise.GeneratePlanar(NoiseExecutor.Left, NoiseExecutor.Right, NoiseExecutor.Top, NoiseExecutor.Bottom);
            }
            else if (projectionMode == 1)
            {
                _noise.GenerateSpherical(NoiseExecutor.South, NoiseExecutor.North, NoiseExecutor.West, NoiseExecutor.East);
            }
            else if (projectionMode == 2)
            {
                _noise.GenerateCylindrical(NoiseExecutor.AngleMin, NoiseExecutor.AngleMax, NoiseExecutor.Top, NoiseExecutor.Bottom);
            }
        }

        public void RenderNormalMap()
        {
            tex = _noise.GetNormalMap(intensity);
        }

        public void StoreFinalizedTexture()
        {
            tex = _noise.GetFinalizedTexture(GradientPresets.Grayscale);
            tex.Apply();
        }

        public void StoreRenderedTexture()
        {
            tex = _noise.GetTexture();
            tex.Apply();
        }

        public void Save(string savePictureName = "")
        {
            if (tex == null) return;

            ImageFileHelpers.SaveToPng(tex, DataPath, savePictureName == "" ? PictureName : savePictureName);
        }
    }
}    