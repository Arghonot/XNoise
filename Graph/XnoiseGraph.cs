using CustomGraph;
using System.Linq;
using UnityEngine;
using LibNoise;
using System;

namespace Xnoise
{
    [CreateAssetMenu(fileName = "XnoiseGraph", menuName = "Graphs/XnoiseGraph", order = 2)]
    public class XnoiseGraph : DefaultGraph, ISerializationCallbackReceiver
    {
        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 512;

        public void Initialize()
        {
            if (this.blackboard == null)
            {
                var bb = this.AddNode<CustomGraph.Blackboard>();
                this.blackboard = bb as CustomGraph.Blackboard;
                this.blackboard.InitializeBlackboard(); // TODO redundant code with graph's inner stuff
            }
            // we do not want to have two outputs
            if (this.root == null && nodes.Any(n => n is RootModuleBase))
            {
                this.root = nodes.OfType<RootModuleBase>().FirstOrDefault();
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

        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize()
        {
            if (blackboard != null && originalStorage != null)
            {
                blackboard.storage = originalStorage;
            }
        }

        public Texture2D Render(bool isGPU = true) // TODO does it belongs here ?
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