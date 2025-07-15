using CustomGraph;
using System.Linq;
using UnityEngine;
using LibNoise;
using System;

namespace XNoise
{
    [CreateAssetMenu(fileName = "XnoiseGraph", menuName = "Xnoise/XnoiseGraph", order = 2)]
    public class XnoiseGraph : GraphBase
    {
        [SerializeField] public Renderer renderer = new Renderer();

        public override Type GetRootNodeType() => typeof(RootModuleBase);

        //public override void Initialize()
        //{
        //    base.Initialize();
        //    if (rootNode == null && nodes.Any(n => n is RootModuleBase))
        //    {
        //        rootNode = nodes.OfType<RootModuleBase>().FirstOrDefault();
        //    }
        //    else if (rootNode == null)
        //    {
        //        rootNode = AddNode<RootModuleBase>();
        //    }
        //}

        public ModuleBase GetGenerator(GraphVariables newstorage = null)
        {
            if (newstorage != null)
            {
                runtimeStorage = newstorage;
            }

            return (ModuleBase)rootNode.GetValue(rootNode.Ports.First());
        }
    }
}