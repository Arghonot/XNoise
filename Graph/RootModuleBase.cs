using System;
using CustomGraph;
using LibNoise;
using UnityEngine;

namespace XNoise
{
    [Serializable]
    [HideFromNodeMenu]
    [NodeTint(XNoiseNodeColors.Output)]
    public class RootModuleBase : Root<ModuleBase>
    {
        public double GetValueCPU(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            throw new NotImplementedException();
        }
    }
}