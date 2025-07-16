using LibNoise;
using System.Diagnostics;
using UnityEngine;
using Xnoise;

namespace XNoise
{
    public class Renderer
    {
        public enum RenderMode
        {
            CPU = 0,
            GPU = 1
        }
        public enum RenderMethod
        {
            HeightMap = 0,
            NormalMap = 1
        }
        public enum ProjectionMode
        {
            Planar = 0,
            Spherical = 1,
            Cylindrical = 2
        }

        [HideInInspector] public static int index = 0;
        // TODO move to a nested class for better clarity
        string DataPath = "/";
        [HideInInspector] public string PictureName = "Test";

        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 0;
        [HideInInspector] public RenderTexture rtex = null;
        [HideInInspector] public Texture2D tex = null;
        [HideInInspector] public Gradient grad = new Gradient();

        [HideInInspector] public RenderMode renderMode;
        [HideInInspector] public RenderMethod renderMethod;
        [HideInInspector] public ProjectionMode projectionMode;

        [HideInInspector] public long RenderTime;

        public float intensity = 10f;

        [HideInInspector] public INoiseStrategy input;


        private NoiseExecutor _noise;

        public Renderer() { }

        public void Render()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            CreateAppropriateNoiseExecutor();
            RenderHeightMap();
            if (renderMethod == RenderMethod.NormalMap) RenderNormalMap();
            if (renderMode == RenderMode.GPU) rtex = ((GPUSurfaceNoiseExecutor)_noise).renderTexture;
            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
        }

        private void CreateAppropriateNoiseExecutor()
        {
            if (renderMode == RenderMode.CPU) _noise = new CPUNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
            else if (renderMode == RenderMode.GPU) _noise = new GPUSurfaceNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
        }

        public void RenderHeightMap()
        {
            if (projectionMode == ProjectionMode.Planar)
            {
                _noise.GeneratePlanar(NoiseExecutor.Left, NoiseExecutor.Right, NoiseExecutor.Top, NoiseExecutor.Bottom);
            }
            else if (projectionMode == ProjectionMode.Spherical)
            {
                _noise.GenerateSpherical(NoiseExecutor.South, NoiseExecutor.North, NoiseExecutor.West, NoiseExecutor.East);
            }
            else if (projectionMode == ProjectionMode.Cylindrical)
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