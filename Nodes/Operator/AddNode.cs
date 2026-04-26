using System.Collections.Generic;
using System.Linq;
using LibNoise;
using LibNoise.Operator;
using XNode;

namespace XNoise
{
    [CreateNodeMenu("NoiseGraph/Combiner/Add")]
    [NodeTint(XNoiseNodeColors.Combiner)]
    public class AddNode : XNoiseNode, IShaderEmitter
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceA;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public ModuleBase SourceB;

        public override object Run()
        {
            Add add = new AddCombiner(
                GetInputValue<ModuleBase>("SourceA", this.SourceA),
                GetInputValue<ModuleBase>("SourceB", this.SourceB));

            return add;
        }

        public string EmitFunction(string funcName, IReadOnlyDictionary<Node, string> childNames)
        {
            var exprA = ChildExpr("SourceA", childNames);
            var exprB = ChildExpr("SourceB", childNames);
            return
$@"float {funcName}(float3 coord)
{{
    return clamp({exprA} + {exprB}, 0.0, 1.0);
}}";
        }

        public IEnumerable<ShaderUniform> Uniforms => Enumerable.Empty<ShaderUniform>();
        public IEnumerable<string> RequiredIncludes => Enumerable.Empty<string>();

        // Returns "fnName(coord)" for connected inputs, or "0.0" as fallback.
        private string ChildExpr(string portName, IReadOnlyDictionary<Node, string> childNames)
        {
            var port = GetInputPort(portName);
            if (port != null && port.IsConnected && childNames.TryGetValue(port.Connection.node, out var fn))
                return $"{fn}(coord)";
            return "0.0";
        }
    }
}
