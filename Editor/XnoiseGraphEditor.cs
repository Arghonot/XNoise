using System;
using UnityEngine;
using XNode;
using XNodeEditor;
using CustomGraph;
using System.Reflection;

namespace XNoise
{
    [CustomNodeGraphEditor(typeof(XnoiseGraph))]
    public class XnoiseGraphEditor : GraphBaseEditor
    {
        public override void RemoveNode(Node node)
        {
            if (node != ((XnoiseGraph)target).blackboard && !node.GetType().ToString().Contains("Root"))
            {
                base.RemoveNode(node);
            }
        }

        public override Texture2D GetGridTexture()
        {
            NodeEditorWindow.current.titleContent = new GUIContent(((XnoiseGraph)target).name);

            return base.GetGridTexture();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (type.GetCustomAttribute<HideFromNodeMenu>(false) == null)
            {
                return base.GetNodeMenuName(type);
            }
             return null;
        }

        //public override void OnCreate()
        //{
        //    base.OnCreate();
        //    XnoiseGraph graph = target as XnoiseGraph;
        //    NodeEditorWindow.current.graphEditor = this;
        //    graph.Initialize();
        //    if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        //}
    }
}