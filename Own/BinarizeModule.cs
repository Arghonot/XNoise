using LibNoise;
using UnityEngine;
using XNoise;

public class BinarizeModule : SerializableModuleBase
{
    #region Constructors

    public BinarizeModule() : base(1) { }

    /// <summary>
    /// Initializes a new instance of Invert.
    /// </summary>
    /// <param name="input">The input module.</param>
    public BinarizeModule(LibNoise.SerializableModuleBase input)
        : base(1)
    {
        Modules[0] = input;
    }

    #endregion

    #region ModuleBase Members

    /// <summary>
    /// Render this generator using a spherical shader.
    /// </summary>
    /// <param name="renderingDatas"></param>
    /// <returns>The generated image.</returns>
    /// 
    /// 
    public override RenderTexture GetValueGPU(GPUSurfaceNoise2d.GPURenderingDatas renderingDatas)
    {
        _materialGPU = XNoiseShaderCache.GetMaterial(XNoiseShaderPaths.Binarize);

        _materialGPU.SetTexture("_TextureA", Modules[0].GetValueGPU(renderingDatas));

        return GPUSurfaceNoise2d.GetImage(_materialGPU, renderingDatas);
    }

    public override double GetValueCPU(double x, double y, double z)
    {
        double val = Modules[0].GetValueCPU(x, y, z);

        return (Mathf.Abs((float)val - 1) > Mathf.Abs((float)val + 1)) ? 1d : -1d;
    }

    #endregion
}
