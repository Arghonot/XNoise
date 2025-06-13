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
    }
}