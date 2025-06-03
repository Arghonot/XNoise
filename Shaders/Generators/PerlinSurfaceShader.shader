Shader "Xnoise/Generators/PerlinSurfaceShader"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Lacunarity("Lacunarity", Float) = 1
        _Persistence("Persistence", Float) = 1
        _Octaves("Octaves", Float) = 1
        _Radius("Radius", Float) = 1.0
        _OffsetPosition("Offset", Vector) = (0, 0, 0, 0)
        _Rotation("Rotation", Vector) = (0, 0, 0, 1)
        _DisplacementMap("Displacement Map", 2D) = "white" {}
        _Seed("Seed", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/noiseSimplex.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/Perlin.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

        sampler2D _DisplacementMap;
        float4 _DisplacementMap_ST;

        float _Frequency;
        float _Lacunarity;
        float _Persistence;
        float _Octaves;
        float _Seed;

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

        float4 FinalColor(float3 coord, float2 uv)
        {
            //float3 rotated = GetRotatedPositions(coord, _OffsetPosition, _Rotation);
            float3 transformedPos = ApplyTransformOperations(coord, uv);
            //float3 displaced = rotated + tex2D(_DisplacementMap, coord.xy).rgb;

            float perlinVal = GetPerlin(transformedPos, _Seed, _Frequency, _Lacunarity, _Persistence, _Octaves);
            float normalized = (perlinVal + 1.0) * 0.5;

            return float4(normalized, normalized, normalized, 1);
        }
        ENDCG

        // ---- PASS 0: PLANAR ----
        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetPlanarCartesianFromUV(i.uv, float3(0, 0, 0));
                return FinalColor(coord, i.uv);
            }
            ENDCG
        }

        // ---- PASS 1: SPHERICAL ----
        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                return FinalColor(coord, i.uv);
            }
            ENDCG
        }

        // ---- PASS 2: CYLINDRICAL ----
        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                float3 coord = GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius);
                return FinalColor(coord, i.uv);
            }
            ENDCG
        }
    }
}
