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
        // todo store me in a class for better wrapping
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
        [HideInInspector] public RenderTexture rtex = null;
        [HideInInspector] public Texture2D tex = null;
        [HideInInspector] public Gradient grad = new Gradient();

        [HideInInspector] public float Space = 110; // todo clean me
        [HideInInspector] public int renderMode; // todo use enum here
        [HideInInspector] public int projectionMode; // todo use enum here

        // todo move to UI logic
        [HideInInspector] public Rect TexturePosition = new Rect(14, 210, 180, 90);

        [HideInInspector] public long RenderTime;


        [HideInInspector] public INoiseStrategy input;
        //private NoiseExecutor _cpuGenerator = new CPUNoiseExecutor();
        //private NoiseExecutor _gpuGenerator = new GPUSurfaceNoiseExecutor();

        private NoiseExecutor _noise;

        public Renderer() { }

        public void Render(bool isgpu = false)
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();


            if (isgpu)
            {
                _noise = new GPUSurfaceNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
            }
            else
            {
                _noise = new CPUNoiseExecutor(width, Height == 0 ? width / 2 : Height, input);
            }

            if (projectionMode == 0)
            {
                _noise.GeneratePlanar(NoiseExecutor.Left, NoiseExecutor.Right, NoiseExecutor.Top, NoiseExecutor.Bottom);
            }
            else if (projectionMode == 1)
            {
                _noise.GenerateSpherical(south, north, west, east);
            }
            else if (projectionMode == 2)
            {
                _noise.GenerateCylindrical(NoiseExecutor.AngleMin, NoiseExecutor.AngleMax, NoiseExecutor.Top, NoiseExecutor.Bottom);
            }

            //watch.Stop();
            //RenderTime = watch.ElapsedMilliseconds;
        }

        public void RenderCPU()
        {
            tex = _noise.GetTexture();
            tex.Apply();
        }

        public void RenderGPU()
        {
            //rtex = _noise.renderTexture;
        }

        public void StoreTex()
        {
            //tex = _noise.GetFinalizedTexture();
        }

        public void Save(string savePictureName = "")
        {
            if (_noise == null) return;

            //ImageFileHelpers.SaveToPng(_noise.GetFinalizedTexture(), DataPath, savePictureName == "" ? PictureName : savePictureName);
        }
    }
}    