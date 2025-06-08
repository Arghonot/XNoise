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
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
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

        float4 PerlinFragment(float3 coord)
        {
            float color = GetPerlin(coord, _Seed, _Frequency, _Lacunarity, _Persistence, _Octaves);

            color = Normalize(color);
            return float4(color, color, color, 1);
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
                return PerlinFragment(GetPointPlanarFromUV(i.uv));
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
                return PerlinFragment(GetPointSphericalFromUV(i.uv));
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
                return PerlinFragment(GetPointCylindricalFromUV(i.uv));
            }
            ENDCG
        }
    }
}
