using System;
using UnityEngine;
using XNode;
using XNodeEditor;
using Graph;
using System.Reflection;

namespace Xnoise
{
    [CustomNodeGraphEditor(typeof(XnoiseGraph))]
    public class LibnoiseGraphEditor : DefaultGraphEditor
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

            else return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            XnoiseGraph graph = target as XnoiseGraph;
            NodeEditorWindow.current.graphEditor = this;

            graph.Initialize();
            //if (graph.blackboard == null)
            //{
            //    var bb = CreateNode(typeof(Graph.Blackboard), new Vector2(0, 0));
            //    graph.blackboard = bb as Graph.Blackboard;
            //    graph.blackboard.InitializeBlackboard(); // TODO redundant code with graph's inner stuff
            //}

            //// we do not want to have two outputs
            //if (graph.root == null && ContainsNodeOfType(typeof(RootModuleBase)) != null)
            //{
            //    graph.root = (RootModuleBase)ContainsNodeOfType(typeof(RootModuleBase));
            //}
            //else if (graph.root == null)
            //{
            //    var root = CreateNode(typeof(RootModuleBase), new Vector2(500, 150));
            //    graph.root = root as RootModuleBase;
            //}
        }
    }
}