using UnityEngine;
using LibNoise;
using Xnoise;

namespace XNoise
{
    public class DisplaceCombiner : LibNoise.Operator.Displace, INoiseStrategy
    {
        private double _influence = 1.0;

        public DisplaceCombiner(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(input, x, y, z) { }
        public DisplaceCombiner(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z, double influence) : base(input, x, y, z)
        {
            _influence = influence;
        }

        public double GetValueCPU(double x, double y, double z) => GetValue(x, y, z);

        public RenderTexture GetValueGPU(GPURenderingDatas datas)
        {
            var materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Displace);

            var tmpDisplacementMap = datas.displacementMap;
            materialGPU.SetTexture("_OriginalDisplacementMap", datas.displacementMap);
            materialGPU.SetTexture("_TextureA", ((INoiseStrategy)Modules[1]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureB", ((INoiseStrategy)Modules[2]).GetValueGPU(datas));
            materialGPU.SetTexture("_TextureC", ((INoiseStrategy)Modules[3]).GetValueGPU(datas));
            materialGPU.SetFloat("_Influence", (float)_influence);
            datas.displacementMap = GPUSurfaceNoiseExecutor.GetImage(materialGPU, datas);
            var render = ((INoiseStrategy)Modules[0]).GetValueGPU(datas);
            datas.displacementMap = tmpDisplacementMap;

            return render;
        }
    }
}