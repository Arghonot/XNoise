Shader "Xnoise/Generators/VoronoiPlanar"
{
    Properties
    {
            _Permutations("Permutations", 2D) = "white" {}
            _Frequency("Frequency",Float) = 0.0
            _Displacement("Displacement",Float) = 0.0
            _Distance("_Distance",Int) = 0
            _Radius("Radius",Float) = 0.0
            _OffsetPosition("Offset", Vector) = (0,0,0,0)
            _Rotation("rotation", Vector) = (0, 0, 0, 1)
            _DisplacementMap("DisplacementMap", 2D) = "white" {}
            _Seed("Seed",Int) = 42
    }
    Subshader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vertex_shader
            #pragma fragment pixel_shader
            #pragma target 3.0

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float _Frequency, _Displacement, _Radius;
            int _Seed, _Distance;
            sampler2D    _Permutations;
            float4 _OffsetPosition, _Rotation;

            #include "UnityCG.cginc"
            #include "../../CGINCs/LibnoiseUtils.cginc"
            #include "../../CGINCs/Voronoi.cginc"

            SamplerState   sampler_Permutations;
            float4 _Permutations_ST;
            sampler2D _DisplacementMap;
            float4 _DisplacementMap_ST;

            v2f vertex_shader(appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _Permutations);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            float4 pixel_shader(v2f ps) : SV_TARGET
            {
                _Radius = _Frequency;
                float3 pos = GetPlanarCartesianFromUV(ps.uv, float3(_OffsetPosition.x,_OffsetPosition.y,_OffsetPosition.z));

                pos = GetRotatedPositions(pos, _OffsetPosition, _Rotation) + tex2D(_DisplacementMap, ps.uv);

                float color = (VoronoiGetValue(pos.x + _OffsetPosition.x, pos.z + _OffsetPosition.y, pos.y + _OffsetPosition.z)) / 2 + 0.5f;

                return float4(color, color, color, 1);
            }

            ENDCG

        }
    }
}