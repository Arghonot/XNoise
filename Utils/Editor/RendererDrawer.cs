using LibNoise;
using UnityEditor;
using UnityEngine;

namespace XNoise
{
    public static class RendererInspectorUI
    {
        public static void Draw(Renderer renderer, ModuleBase input, bool isNodeUI)
        {
            renderer.renderMode = GUILayout.Toolbar(renderer.renderMode, new string[] { "CPU", "GPU" });
            renderer.projectionMode = EditorGUILayout.Popup(renderer.projectionMode, new string[] { "Planar", "Spherical", "Cylindrical" });
            if (GUILayout.Button("Render"))
            {
                if (input != null)
                {
                    renderer.input = input;
                    renderer.Render();
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

            if (isNodeUI) GUILayout.Space(renderer.Space);

            if (renderer.tex != null)
            {
                if (isNodeUI)
                {
                    GUI.DrawTexture(renderer.TexturePosition, renderer.tex);
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