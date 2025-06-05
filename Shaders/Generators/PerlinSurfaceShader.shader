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
        _Seed("Seed", Float) = 1
        _TurbulenceMap("Turbulence Map", 2D) = "white" {}
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

        //sampler2D _TurbulenceMap;
        //float4 _TurbulenceMap_ST;

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
            float3 transformedPos = ApplyTransformOperations(coord, uv);
            float perlinVal = GetPerlin(transformedPos, _Seed, _Frequency, _Lacunarity, _Persistence, _Octaves);

            perlinVal = Normalize(perlinVal);
            return float4(perlinVal, perlinVal, perlinVal, 1);
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
                return FinalColor(GetPlanarCartesianFromUV(i.uv, _OffsetPosition.xyz), i.uv);
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
                return FinalColor(GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius), i.uv);
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
                return FinalColor(GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius), i.uv);
            }
            ENDCG
        }
    }
}
