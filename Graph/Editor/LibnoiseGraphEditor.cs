using GraphEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using Graph;

namespace Xnoise
{
    [CustomNodeGraphEditor(typeof(XnoiseGraph))]
    public class LibnoiseGraphEditor : DefaultGraphEditor
    {
        // TODO find a way to make this inherit generic node graph editor as well
        // perhaps use attribute Don't show in dialogue
        List<Type> HiddenTypes = new List<Type>()
        {
            typeof(RootModuleBase),
            typeof(Graph.Blackboard),
            typeof(Graph.Single),
            typeof(LibnoiseNode),
            typeof(Graph.NodeBase)
        };

        public override void RemoveNode(Node node)
        {
            if (node != ((XnoiseGraph)target).blackboard &&
                !node.GetType().ToString().Contains("Root"))
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
            if (!HiddenTypes.Contains(type) &&
                !type.ToString().Contains("Root") &&
                !type.ToString().Contains("[T]"))
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

            if (graph.blackboard == null)
            {
                var bb = CreateNode(typeof(Graph.Blackboard), new Vector2(0, 0));
                graph.blackboard = bb as Graph.Blackboard;
                graph.blackboard.InitializeBlackboard();
            }
            
            // we do not want to have two outputs
            if (graph.root == null && ContainsNodeOfType(typeof(RootModuleBase)) != null)
            {
                graph.root = (RootModuleBase)ContainsNodeOfType(typeof(RootModuleBase));
            }
            else if (graph.root == null)
            {
                var root = CreateNode(typeof(RootModuleBase), new Vector2(500, 150));
                graph.root = root as RootModuleBase;
            }
        }


    }
}