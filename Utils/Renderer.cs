using LibNoise;
using System.Diagnostics;
using UnityEngine;

namespace XNoise
{
    public class Renderer
    {
        [HideInInspector] public static int index = 0;
        string DataPath = "/";
        [HideInInspector] public string PictureName = "Test";
        [HideInInspector] public float south = 90.0f;
        [HideInInspector] public float north = -90.0f;
        [HideInInspector] public float west = -180.0f;
        [HideInInspector] public float east = 180.0f;
        [HideInInspector] public float angleMin = -180.0f;   // or 0.0f
        [HideInInspector] public float angleMax = 180.0f;    // or 360.0
        [HideInInspector] public float heightMin = -1.0f;    // bottom of the cylinder
        [HideInInspector] public float heightMax = 1.0f;     // top of the cylinder

        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 0;
        [HideInInspector] public Texture2D tex = null;
        [HideInInspector] public Gradient grad = new Gradient();

        [HideInInspector] public float Space = 110; // clean me
        [HideInInspector] public int renderMode;
        [HideInInspector] public int projectionMode;

        [HideInInspector] public Rect TexturePosition = new Rect(14, 210, 180, 90);

        [HideInInspector] public long RenderTime;


        [HideInInspector] public SerializableModuleBase input;

        public Renderer()
        {

        }

        public void Render()
        {
            if (renderMode == 0)
            {
                RenderCPU();
            }
            if (renderMode == 1)
            {
                RenderGPU();
            }
        }

        public void RenderCPU()
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            Noise2D map = new Noise2D(width, Height == 0 ? width / 2 : Height, input);

            if (projectionMode == 0)
            {
                map.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);
            }
            else if (projectionMode == 1)
            {
                map.GenerateSpherical(
                    south,
                    north,
                    west,
                    east);
            }
            else if (projectionMode == 2)
            {
                map.GenerateCylindrical(Noise2D.AngleMin, Noise2D.AngleMax, Noise2D.Top, Noise2D.Bottom);
            }

            tex = map.GetTexture();
            tex.Apply();

            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
        }

        public void RenderGPU()
        {
            index = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var map = new Noise2D(width, Height == 0 ? width / 2 : Height, input);
            map.useGPU = true;

            if (projectionMode == 0)
            {
                map.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);
            }
            else if (projectionMode == 1)
            {
                map.GenerateSpherical(south, north, west, east);
            }
            else if (projectionMode == 2)
            {
                map.GenerateCylindrical(angleMin, angleMax, heightMin, heightMax);
            }

            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
            tex = map.GetTextureVisualization();
        }

        public void Save()
        {
            if (tex == null) return;

            ImageFileHelpers.SaveToPng(tex, DataPath, PictureName);
        }
    }
}    