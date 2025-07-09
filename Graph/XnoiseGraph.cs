using CustomGraph;
using System.Linq;
using UnityEngine;
using LibNoise;
using System;

namespace XNoise
{
    [CreateAssetMenu(fileName = "XnoiseGraph", menuName = "Graphs/XnoiseGraph", order = 2)]
    public class XnoiseGraph : GraphBase, ISerializationCallbackReceiver
    {
        [SerializeField] public Renderer renderer = new Renderer();

        public override Type GetRootNodeType() => typeof(RootModuleBase);

        public override void Initialize()
        {
            base.Initialize();
            if (rootNode == null && nodes.Any(n => n is RootModuleBase))
            {
                rootNode = nodes.OfType<RootModuleBase>().FirstOrDefault();
            }
            else if (rootNode == null)
            {
                rootNode = AddNode<RootModuleBase>();
            }
        }

        public SerializableModuleBase GetGenerator(GraphVariables newstorage = null)
        {
            if (newstorage != null)
            {
                runtimeStorage = newstorage; // changed a pretty bad typo here does it still work ?
            }

            return (SerializableModuleBase)rootNode.GetValue(rootNode.Ports.First());
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