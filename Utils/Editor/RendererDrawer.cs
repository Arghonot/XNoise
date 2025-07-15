using LibNoise;
using UnityEditor;
using UnityEngine;

namespace XNoise
{
    public static class RendererInspectorUI
    {

        [HideInInspector] public static readonly float Space = 110; // todo clean me
        [HideInInspector] public static readonly Rect TexturePosition = new Rect(14, 230, 180, 90);

        public static void Draw(Renderer renderer, ModuleBase input, bool isNodeUI)
        {
            renderer.renderMode = (Renderer.RenderMode)GUILayout.Toolbar((int)renderer.renderMode, new string[] { "CPU", "GPU" });
            renderer.renderMethod = (Renderer.RenderMethod)GUILayout.Toolbar((int)renderer.renderMethod, new string[] { "HeightMap", "NormalMap" });
            renderer.projectionMode = (Renderer.ProjectionMode)EditorGUILayout.Popup((int)renderer.projectionMode, new string[] { "Planar", "Spherical", "Cylindrical" });
            if (GUILayout.Button("Render"))
            {
                if (input != null)
                {
                    renderer.input = input as INoiseStrategy;
                    renderer.Render();
                    if (renderer.renderMethod == 0) renderer.StoreFinalizedTexture();
                }
            }
            if (GUILayout.Button("Save"))
            {
                renderer.Save();
            }
            renderer.PictureName = GUILayout.TextField(renderer.PictureName);

            GUILayout.Space(5);

            renderer.width = EditorGUILayout.IntField("Size ", renderer.width);
            GUILayout.Label("Render time (ms) : " + renderer.RenderTime.ToString());

            if (isNodeUI) GUILayout.Space(Space);

            if (renderer.tex != null)
            {
                if (isNodeUI)
                {
                    GUI.DrawTexture(TexturePosition, renderer.tex);
                }
                else
                {
                    float availableWidth = EditorGUIUtility.currentViewWidth - 40f;
                    float aspectRatio = (float)renderer.tex.height / renderer.tex.width;
                    float height = availableWidth * aspectRatio;

                    Rect textureRect = GUILayoutUtility.GetRect(availableWidth, height, GUILayout.ExpandWidth(false));
                    GUI.DrawTexture(textureRect, renderer.tex, ScaleMode.ScaleToFit, false);
                }
            }
        }
    }
}