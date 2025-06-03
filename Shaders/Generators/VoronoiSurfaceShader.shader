Shader "Xnoise/Generators/VoronoiSurfaceShader"
{
    Properties
    {
        _Permutations("Permutations", 2D) = "white" {}
        _Frequency("Frequency", Float) = 0.0
        _Displacement("Displacement", Float) = 0.0
        _Distance("_Distance", Int) = 0
        _Radius("Radius", Float) = 0.0
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("rotation", Vector) = (0, 0, 0, 1)
        _DisplacementMap("DisplacementMap", 2D) = "white" {}
        _Seed("Seed", Int) = 42
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/Voronoi.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

        sampler2D _DisplacementMap;
        float4 _DisplacementMap_ST;
        sampler2D _Permutations;
        float4 _Permutations_ST;

        float _Frequency, _Displacement;
        int _Seed, _Distance;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        float4 FinalColor(float3 coord)
        {
            _Radius = _Frequency;

            float3 rotated = GetRotatedPositions(coord, _OffsetPosition.xyz, _Rotation);
            float3 displaced = rotated + tex2D(_DisplacementMap, coord.xy);
            float3 position = float3(displaced.x + _OffsetPosition.x, displaced.z + _OffsetPosition.y, displaced.y + _OffsetPosition.z);

            float val = VoronoiGetValue(position, _Seed, _Frequency, _Distance, _Displacement);
            float normalized = (val + 1) * 0.5;

            return float4(normalized, normalized, normalized, 1);
        }
        ENDCG

        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetPlanarCartesianFromUV(i.uv, _OffsetPosition.xyz);
                return FinalColor(coord);
            }
            ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                return FinalColor(coord);
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetCylindricalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                return FinalColor(coord);
            }
            ENDCG
        }
    }
}
