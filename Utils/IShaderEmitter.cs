using System.Collections.Generic;
using XNode;

namespace XNoise
{
    /// <summary>
    /// Stores one HLSL uniform that a node needs in the generated shader.
    /// Name is the HLSL identifier (with leading underscore, e.g. "_Freq1").
    /// Default is the value from the blackboard's originalStorage, used for the Properties block.
    /// </summary>
    public readonly struct ShaderUniform
    {
        public readonly string Name;
        public readonly float Default;
        public ShaderUniform(string name, float defaultValue) { Name = name; Default = defaultValue; }
    }

    /// <summary>
    /// Implemented by XNode nodes that can participate in single-pass shader compilation.
    /// All emitted functions take float3 coord — coordinate mapping happens once in the frag.
    /// </summary>
    public interface IShaderEmitter
    {
        /// <summary>
        /// Emits a complete HLSL function: float funcName(float3 coord) { ... }
        /// childNames maps each input XNode.Node child to its assigned function name.
        /// </summary>
        string EmitFunction(string funcName, IReadOnlyDictionary<Node, string> childNames);

        /// <summary>
        /// Uniform variables this node contributes (blackboard-connected parameters).
        /// The generator deduplicates across all nodes.
        /// </summary>
        IEnumerable<ShaderUniform> Uniforms { get; }

        /// <summary>
        /// Filenames within Assets/XNoise/Shaders/CGINCs/ needed by this node.
        /// </summary>
        IEnumerable<string> RequiredIncludes { get; }
    }
}
