using Graph;
using System.Linq;
using UnityEngine;
using LibNoise;
using System;
using static TreeEditor.TreeEditorHelper;
using XNodeEditor;

namespace Xnoise
{
    [CreateAssetMenu(fileName = "XnoiseGraph", menuName = "Graphs/XnoiseGraph", order = 2)]
    public class XnoiseGraph : DefaultGraph, ISerializationCallbackReceiver
    {
        [HideInInspector] public float south = 90.0f;
        [HideInInspector] public float north = -90.0f;
        [HideInInspector] public float west = -180.0f;
        [HideInInspector] public float east = 180.0f;
        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 512;

        public void Initialize()
        {
            if (this.blackboard == null)
            {
                var bb = this.AddNode<Graph.Blackboard>();
                this.blackboard = bb as Graph.Blackboard;
                this.blackboard.InitializeBlackboard(); // TODO redundant code with graph's inner stuff
            }
            // we do not want to have two outputs
            if (this.root == null && nodes.Any(n => n is RootModuleBase))//ContainsNodeOfType(typeof(RootModuleBase)) != null)
            {
                this.root = nodes.OfType<RootModuleBase>().FirstOrDefault();//(RootModuleBase)ContainsNodeOfType(typeof(RootModuleBase));
            }
            else if (this.root == null)
            {
                this.root = this.AddNode<RootModuleBase>();
            }
        }

        public override Type GetRootNodeType() => typeof(RootModuleBase);

        public SerializableModuleBase GetGenerator(GraphVariableStorage newstorage = null)
        {
            if (newstorage != null)
            {
                this.originalStorage = newstorage;
            }

            return (SerializableModuleBase)root.GetValue(root.Ports.First());
        }

        public void OnAfterDeserialize()
        {
            // nothing to do there
        }

        public void OnBeforeSerialize()
        {
            if (blackboard != null && originalStorage != null)
            {
                blackboard.storage = originalStorage;
            }
        }

        public Texture2D Render(bool isGPU = true)
        {
            Texture2D tex;
            var generator = GetGenerator();
            Noise2D map = new Noise2D(width, Height == 0 ? width / 2 : Height, generator);

            map.useGPU = isGPU;
            map.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);
            tex = map.GetTexture();
            tex.Apply();
            return tex;
        }
    }    
}