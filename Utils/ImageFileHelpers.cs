using System.IO;
using UnityEngine;

namespace XNoise
{
    public static class ImageFileHelpers 
    {
        public static void SaveToPng(Texture2D tex, string DataPath, string PictureName)
        {
            Renderer.index++;
            UnityEngine.Debug.Log(Application.dataPath + DataPath + PictureName + ".png");
            File.WriteAllBytes(Application.dataPath + DataPath + PictureName + ".png", tex.EncodeToPNG());
        }
        public static void SaveToJPG(Texture2D tex, string DataPath, string PictureName)
        {
            Renderer.index++;
            UnityEngine.Debug.Log(Application.dataPath + DataPath + PictureName + ".jpg");
            File.WriteAllBytes(Application.dataPath + DataPath + PictureName + ".jpg", tex.EncodeToJPG());
        }

        public static Texture2D toTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        public static Texture2D BlitMaterialToTexture(
            Material mat,
            int width = 256,
            int height = 256,
            RenderTextureFormat rtFormat = RenderTextureFormat.ARGB32,
            TextureWrapMode wrap = TextureWrapMode.Clamp,
            FilterMode filter = FilterMode.Bilinear,
            bool mipmaps = false)
        {
            RenderTexture rt = RenderTexture.GetTemporary(
                width, height, depthBuffer: 0, rtFormat,
                RenderTextureReadWrite.sRGB);

            Graphics.Blit(source: null, dest: rt, mat);

            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D tex = new Texture2D(
                width, height, TextureFormat.RGBA32, mipmaps, linear: false)
            {
                wrapMode = wrap,
                filterMode = filter
            };

            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, recalculateMipMaps: false);
            tex.Apply(updateMipmaps: mipmaps, makeNoLongerReadable: false);

            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);

            return tex;
        }
    }
}