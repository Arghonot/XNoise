using System.Collections.Generic;
using UnityEngine;

namespace XNoise
{
    // todo implement me for V2
    public class RenderTextureCollection : MonoBehaviour
    {
        public static Stack<RenderTexture> rdbs = new Stack<RenderTexture>();
        public static Stack<RenderTexture> usedRdbs;

        public static void StartStacking()
        {
            rdbs = new Stack<RenderTexture>();
        }

        public static void AddToStack(RenderTexture rdb)
        {
            //rdbs.Push(rdb);
            //rdb.DiscardContents();
        }

        public static void StopStacking()
        {
            //usedRdbs = new List<RenderTexture>();
        }

        public static RenderTexture GetFromStack(Vector2 size)
        {
            //if (rdbs.Count > 0)
            //{
            //    return rdbs.Pop();
            //}

            RenderTexture rt = new RenderTexture((int)size.x, (int)size.y, 0, RenderTextureFormat.RFloat);

            rt.filterMode = FilterMode.Point;
            rt.useMipMap = false;
            rt.wrapMode = TextureWrapMode.Clamp;
            rt.Create();

            return rt;
        }
    }
}