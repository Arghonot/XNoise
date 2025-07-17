using System.Collections.Generic;
using UnityEngine;

namespace XNoise
{
    public static class UtilsFunctions
    {
        public static int TextureSize = 1000;

        public static Texture2D GetCurveAsTexture(AnimationCurve curve)
        {
            Texture2D curveTexture = new Texture2D(TextureSize, 1);
            float currentValue = 0f;

            for (int i = 0; i < TextureSize; i++)
            {
                currentValue = (Mathf.Clamp(
                    curve.Evaluate(
                        Mathf.Lerp(-1f, 1f, (float)i / (float)TextureSize)),
                        -1f,
                        1f) + 1f) / 2f;
                curveTexture.SetPixel(i, 0, new Color(currentValue, currentValue, currentValue));
            }
            curveTexture.Apply();

            return curveTexture;
        }

        public static Texture2D GetTerracePointListAsTexture(List<double> _data)
        {
            Texture2D pointList = new Texture2D(_data.Count + 1, 1);

            pointList.SetPixel(0, 0, new Color(((float)_data.Count) / 255f, 0, 0));

            for (int i = 1; i < _data.Count + 1; i++)
            {
                float channelValue = ((float)_data[i - 1] + 1f) / 2f;
                pointList.SetPixel(i, 0, new Color(channelValue, channelValue, channelValue, 1f));
            }

            pointList.Apply();

            return pointList;
        }

        public static Texture2D GetGradientAsTexture(Gradient gradient)
        {
            Texture2D gradientTexture = new Texture2D(TextureSize, 1);

            for (int i = 0; i < TextureSize; i++)
            {
                gradientTexture.SetPixel(i, 0, gradient.Evaluate((float)i / (float)TextureSize));
            }
            gradientTexture.Apply();

            return gradientTexture;
        }
    }
}