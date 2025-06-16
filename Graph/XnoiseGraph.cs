using CustomGraph;
using System.Linq;
using UnityEngine;
using LibNoise;
using System;

namespace XNoise
{
    [CreateAssetMenu(fileName = "XnoiseGraph", menuName = "Graphs/XnoiseGraph", order = 2)]
    public class XnoiseGraph : DefaultGraph, ISerializationCallbackReceiver
    {
        [SerializeField] public Renderer renderer = new Renderer();

        [HideInInspector] public int width = 512;
        [HideInInspector] public int Height = 512;

        public void Initialize()
        {
            if (this.blackboard == null)
            {
                var bb = this.AddNode<CustomGraph.Blackboard>();
                this.blackboard = bb as CustomGraph.Blackboard;
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

        public SerializableModuleBase GetGenerator(GraphVariables newstorage = null)
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
    }
}