using UnityEngine;
using XNoise;

namespace Xnoise
{
    public class GPUSurfaceNoiseExecutor : NoiseExecutor
    {
        #region Fields

        public GPURenderingDatas datas;
        public Vector3 origin;
        public RenderTexture renderTexture { get => _renderedTexture; }
        private RenderTexture _renderedTexture;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of GPUSurfaceNoiseExecutor.
        /// </summary>
        protected GPUSurfaceNoiseExecutor() : base() { }

        /// <summary>
        /// Initializes a new instance of GPUSurfaceNoiseExecutor.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        public GPUSurfaceNoiseExecutor(int size) : base(size, size, null) { }

        /// <summary>
        /// Initializes a new instance of GPUSurfaceNoiseExecutor.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public GPUSurfaceNoiseExecutor(int size, INoiseStrategy generator) : base(size, size, generator) { }

        /// <summary>
        /// Initializes a new instance of GPUSurfaceNoiseExecutor.
        /// </summary>
        /// <param name="width">The width of the noise map.</param>
        /// <param name="height">The height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public GPUSurfaceNoiseExecutor(int width, int height, INoiseStrategy generator = null) : base(width, height) { _generator = generator; }

        #endregion

        public override void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true) => GeneratePlanar(left, right, top, bottom, isSeamless, null);
        public void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true, Texture2D texture2d = null)
        {
            base.GeneratePlanar(left, right, top, bottom, isSeamless);
            datas = new GPURenderingDatas(new Vector2(Width, Height), ProjectionType.Planar, RenderingAreaData.standardCartesian);

            if (texture2d != null)
            {
                RenderTexture rt = new RenderTexture(texture2d.width / 2, texture2d.height / 2, 0);
                RenderTexture.active = rt;
                // Copy your texture ref to the render texture
                Graphics.Blit(texture2d, rt);
                datas.displacementMap = rt;
            }
            // set texture here
            _renderedTexture = _generator.GetValueGPU(datas);
        }

        public override void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
        {
            base.GenerateCylindrical(angleMin, angleMax, heightMin, heightMax);
            GPURenderingDatas datas = new GPURenderingDatas(new Vector2(Width, Height), ProjectionType.Cylindrical, RenderingAreaData.standardCylindrical);
            _renderedTexture = _generator.GetValueGPU(datas);
        }

        public override void GenerateSpherical(double south, double north, double west, double east)
        {
            base.GenerateSpherical(south, north, west, east);
            GPURenderingDatas datas = new GPURenderingDatas(new Vector2(Width, Height), ProjectionType.Spherical, RenderingAreaData.standardSpherical);

            _renderedTexture = _generator.GetValueGPU(datas);
        }

        public override Texture2D GetNormalMap(float intensity)
        {
            RenderTexture preview = new RenderTexture(_renderedTexture.width, _renderedTexture.height, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = preview;
            var mat = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.ToNormalMap);

            mat.SetTexture("_HeightMap", _renderedTexture);
            mat.SetFloat("_Intensity", intensity);
            mat.SetVector("_TexelSize", new Vector4(_renderedTexture.width, _renderedTexture.height, 0, 0));

            Graphics.Blit(_renderedTexture, preview, mat);

            var tex = new Texture2D(_renderedTexture.width, _renderedTexture.height);
            tex.ReadPixels(new Rect(0, 0, _renderedTexture.width, _renderedTexture.height), 0, 0);
            tex.Apply();

            return tex;
        }

        /// <summary>
        /// Creates a single channel texture map for the current content of the noise map.
        /// </summary>
        /// <returns>The created texture map.</returns>
        public override Texture2D GetTexture()
        {
            RenderTexture.active = _renderedTexture;
            var tex = new Texture2D(_renderedTexture.width, _renderedTexture.height, TextureFormat.R8, false);
            tex.ReadPixels(new Rect(0, 0, _renderedTexture.width, _renderedTexture.height), 0, 0);
            tex.Apply();

            return tex;
        }

        public override Texture2D GetFinalizedTexture(object gradient)
        {
            RenderTexture preview = new RenderTexture(_renderedTexture.width, _renderedTexture.height, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = preview;
            var mat = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Visualizer);

            mat.SetTexture("_Input", _renderedTexture);
            Graphics.Blit(_renderedTexture, preview, mat);
            var tex = new Texture2D(_renderedTexture.width, _renderedTexture.height);
            tex.ReadPixels(new Rect(0, 0, _renderedTexture.width, _renderedTexture.height), 0, 0);
            tex.Apply();

            return tex;
        }

        public RenderTexture GetRenderTexture()
        {
            RenderTexture.active = _renderedTexture;

            return _renderedTexture;
        }

        public static RenderTexture GetImage(Material material, GPURenderingDatas renderingDatas, bool isGenerator = false)
        {
            if (isGenerator)
            {
                material.SetVector("_Rotation", renderingDatas.quaternionRotation);
                material.SetVector("_OffsetPosition", renderingDatas.origin);
                material.SetFloat("_Radius", 1f);
                material.SetFloat("_TurbulencePower", renderingDatas.turbulencePower);
                material.SetVector("_Scale", renderingDatas.scale);
                material.SetTexture("_TurbulenceMap", renderingDatas.displacementMap);
            }

            RenderTexture rdB = RenderTextureCollection.GetFromStack(renderingDatas.size);
            RenderTexture.active = rdB;
            Graphics.Blit(Texture2D.blackTexture, rdB, material, isGenerator ? (int)renderingDatas.projection : 0);
            RenderTextureCollection.AddToStack(rdB);

            return rdB;
        }

        public static Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}