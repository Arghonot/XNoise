using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace XNoise
{
    [CustomNodeEditor(typeof(Renderer))]
    public class RendererEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            Renderer rend = target as Renderer;

            rend.renderMode = GUILayout.Toolbar(rend.renderMode, new string[] { "CPU", "GPU" });
            rend.projectionMode = EditorGUILayout.Popup(rend.projectionMode, new string[] { "Planar", "Spherical", "Cylindrical" });
            if (GUILayout.Button("Render"))
            {
                rend.Render();
            }
            if (GUILayout.Button("Save"))
            {
                rend.Save();
            }
            rend.PictureName = GUILayout.TextField(rend.PictureName);

            GUILayout.Space(5);

            DisplayInputFromName("Input");
            rend.width = EditorGUILayout.IntField("Size ", rend.width);
            GUILayout.Label("Render time (ms) : " + rend.RenderTime.ToString());

            GUILayout.Space(rend.Space);

            if (rend.tex != null)
            {
                GUI.DrawTexture(rend.TexturePosition, rend.tex);
            }
        }

        private void DisplayInputFromName(string name)
        {
            string[] excludes = { "m_Script", "graph", "position", "ports" }; // Todo fix me
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                if (iterator.name == name)
                {
                    NodeEditorGUILayout.PropertyField(iterator, true);
                }
            }
        }
    }
}