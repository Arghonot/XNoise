Shader "Xnoise/Generators/RidgedMultifractalSurfaceShader"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Lacunarity("Lacunarity", Float) = 1
        _Octaves("Octaves", Float) = 1
        _Radius("Radius", Float) = 1
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("rotation", Vector) = (0, 0, 0, 1)
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
        _Seed("Seed", Int) = 42
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/noiseSimplex.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/RidgedMultifractal.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

        sampler2D _DisplacementMap;
        float _Frequency, _Lacunarity, _Octaves;
        int _Seed;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        float4 ComputeRidged(float3 coord)
        {
            float3 rotated = GetRotatedPositions(coord, _OffsetPosition, _Rotation);
            float3 displaced = rotated + tex2D(_DisplacementMap, coord.xy).rgb;
            float value = GetRidgedMultifractal(displaced, _Frequency, _Lacunarity, _Octaves);
            float normalized = (value + 1.0) * 0.5;
            return float4(normalized, normalized, normalized, 1);
        }
        ENDCG

        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_planar

            float4 frag_planar(v2f i) : SV_Target
            {
                float3 coord = GetPlanarCartesianFromUV(i.uv, _OffsetPosition.xyz);
                return ComputeRidged(coord);
            }
        ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_spherical

            float4 frag_spherical(v2f i) : SV_Target
            {
                float3 coord = GetSphericalCartesianFromUV(i.uv, _Radius);
                return ComputeRidged(coord);
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_cylindrical

            float4 frag_cylindrical(v2f i) : SV_Target
            {
                float3 coord = GetCylindricalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                return ComputeRidged(coord);
            }
            ENDCG
        }
    }
}
