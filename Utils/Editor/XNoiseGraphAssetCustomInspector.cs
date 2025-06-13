using UnityEditor;
using UnityEngine;

namespace XNoise
{
    [CustomEditor(typeof(XnoiseGraph))]
    public class XNoiseGraphAssetCustomInspector : Editor
    {
        private bool folded;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            XnoiseGraph graph = target as XnoiseGraph;
            GUILayout.Space(5);

            GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout);
            boldFoldout.fontStyle = FontStyle.Bold;
            folded = EditorGUILayout.Foldout(folded, "Graph Visualizer", true, boldFoldout);
            
            if (folded)
            {
                EditorGUI.indentLevel++;
                RendererInspectorUI.Draw(graph.renderer, graph.GetGenerator(), false);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}