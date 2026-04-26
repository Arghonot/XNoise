using System;
using System.Collections.Generic;
using System.Globalization;
using CustomGraph;
using LibNoise;
using XNode;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Generator/Perlin")]
    [NodeTint(XNoiseNodeColors.Generator)]
    public class PerlinNode : XNoiseNode, IShaderEmitter
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double frequency = 3;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double lacunarity = 2;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public double persistence = 0.5;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Octaves = 6;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Seed;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public QualityMode Quality;

        public override object Run()
        {
            return new PerlinGenerator(
                GetInputValue<double>("frequency", this.frequency),
                GetInputValue<double>("lacunarity", this.lacunarity),
                GetInputValue<double>("persistence", this.persistence),
                GetInputValue<int>("Octaves", this.Octaves),
                GetInputValue<int>("Seed", this.Seed),
                GetInputValue<QualityMode>("Quality", this.Quality));
        }

        public string EmitFunction(string funcName, IReadOnlyDictionary<Node, string> childNames)
        {
            return
            $@"float {funcName}(float3 coord)
            {{
                return Normalize(GetPerlin(coord, {Param("Seed", (double)Seed)}, {Param("frequency", frequency)}, {Param("lacunarity", lacunarity)}, {Param("persistence", persistence)}, {Param("Octaves", (double)Octaves)}));
            }}";
        }

        public IEnumerable<ShaderUniform> Uniforms
        {
            get
            {
                foreach (var portName in new[] { "Seed", "frequency", "lacunarity", "persistence", "Octaves" })
                {
                    var port = GetInputPort(portName);
                    if (port != null && port.IsConnected && port.Connection.node is BlackBoardVariable bb)
                    {
                        var raw = ((GraphBase)graph).originalStorage.GetFromGUID(bb.guid);
                        float def = raw != null ? Convert.ToSingle(raw) : 0f;
                        yield return new ShaderUniform($"_{bb.variableName}", def);
                    }
                }
            }
        }

        public IEnumerable<string> RequiredIncludes => new[]
        {
            "noiseSimplex.cginc",
            "LibnoiseUtils.cginc",
            "Perlin.cginc"
        };

        // Returns the HLSL expression for a parameter: uniform name if blackboard-connected,
        // otherwise the backing value inlined as a float literal.
        private string Param(string portName, double backingValue)
        {
            var port = GetInputPort(portName);
            if (port != null && port.IsConnected && port.Connection.node is BlackBoardVariable bb)
                return $"_{bb.variableName}";
            return backingValue.ToString("F6", CultureInfo.InvariantCulture);
        }
    }
}
