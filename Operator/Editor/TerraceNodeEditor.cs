//using LibNoise;
//using System;
//using System.Linq;
//using UnityEditor;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using XNodeEditor;
//using Xnoise;

//namespace GraphEditor
//{
//    [CustomNodeEditor(typeof(Xnoise.TerraceNode))]
//    public class TerraceNodeEditor : XNodeEditor.NodeEditor
//    {
//        int index = 0;
//        public override void OnBodyGUI()
//        {
//            serializedObject.Update();
//            string[] excludes = { "m_Script", "graph", "position", "ports" };

//            //DisplayInputFromName("InputModule");

//            //Debug.Log("================");
//            //// Inputs
//            //// Iterate through serialized properties and draw them like the Inspector (But with ports)
//            //SerializedProperty iterator = serializedObject.GetIterator();
//            //bool enterChildren = true;
//            //while (iterator.NextVisible(enterChildren))
//            //{
//            //    enterChildren = false;
//            //    Debug.Log(iterator.name);
//            //    if (excludes.Contains(iterator.name)) continue;
//            //    NodeEditorGUILayout.PropertyField(iterator, true);
//            //}
//            //Debug.Log("================");

//            //// outputs
//            // Iterate through dynamic ports and draw them in the order in which they are serialized
//            //foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
//            //{
//            //    if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
//            //    NodeEditorGUILayout.PortField(dynamicPort);
//            //}

//            //serializedObject.ApplyModifiedProperties();

//            index = EditorGUILayout.Popup(index, new string[]
//            {
//                "Double[]",
//                "Curve"
//            });
//            serializedObject.Update();

//            DisplayInputFromName("InputModule");

//            if (index != ((TerraceNode)target).mode)
//            {
//                ((TerraceNode)target).mode = index;
//            }
//            if (index == 0)
//            {
//                // display array inputs
//                AddTerraceLevelGUI();
//                DisplayDoublePorts();
//                //serializedObject.Update();
//            }
//            else
//            {
//                // display curve input
//                DisplayCurvePort();
//            }

//            DisplayDynamicPortsOfType(typeof(SerializableModuleBase));

//            serializedObject.ApplyModifiedProperties();
//        }

//        private void DisplayDoublePorts()
//        {
//            DisplayDynamicPortsOfType(typeof(double));
//        }

//        private void DisplayCurvePort()
//        {
//            DisplayInputFromName("curve");
//        }

//        private void DisplayInputFromName(string name)
//        {
//            string[] excludes = { "m_Script", "graph", "position", "ports" };
//            SerializedProperty iterator = serializedObject.GetIterator();
//            bool enterChildren = true;
//            while (iterator.NextVisible(enterChildren))
//            {
//                enterChildren = false;
//                if (excludes.Contains(iterator.name)) continue;
//                if (iterator.name == name)
//                {
//                    NodeEditorGUILayout.PropertyField(iterator, true);
//                }
//            }
//        }

//        private void DisplayDynamicPortsOfType(Type portVariableType)
//        {
//            // Iterate through dynamic ports and draw them in the order in which they are serialized
//            foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
//            {
//                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
//                if (dynamicPort.ValueType == portVariableType)
//                {
//                    NodeEditorGUILayout.PortField(dynamicPort);
//                }
//            }
//        }

//        private void AddTerraceLevelGUI()
//        {
//            if (GUILayout.Button("Add level"))
//            {
//                AddTerraceLevel();
//            }
//            if ((target as TerraceNode).TerraceDoubles.Count != 0)
//            {
//                if (GUILayout.Button("Remove level"))
//                {
//                    RemoveTerraceLevel();
//                }
//            }
//        }

//        private void RemoveTerraceLevel()
//        {
//            TerraceNode terrace = target as TerraceNode;

//            terrace.RemoveDynamicPort(terrace.TerraceDoubles.Count.ToString());
//            terrace.TerraceDoubles.RemoveAt(terrace.TerraceDoubles.Count - 1);
//        }

//        private void AddTerraceLevel()
//        {
//            TerraceNode terrace = target as TerraceNode;

//            terrace.TerraceDoubles.Add(0d);

//            terrace.AddDynamicInput(typeof(double), XNode.Node.ConnectionType.Override, XNode.Node.TypeConstraint.Strict, terrace.TerraceDoubles.Count.ToString());
//        }
//    }
//}