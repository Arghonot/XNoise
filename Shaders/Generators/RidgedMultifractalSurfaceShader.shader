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

        float GetColor(float3 coord)
        {
            float value = GetRidgedMultifractal(coord, _Seed, _Frequency, _Lacunarity, _Octaves);
            return (value + 1.0) * 0.5;
        }
        ENDCG

        Pass
        {
            Name "PLANAR"
            Tags { "Projection" = "Planar" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_planar

            float frag_planar(v2f i) : SV_Target
            {
                return GetColor(GetPointPlanarFromUV(i.uv));
            }
        ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            Tags { "Projection" = "Spherical" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_spherical

            float frag_spherical(v2f i) : SV_Target
            {
                return GetColor(GetPointSphericalFromUV(i.uv));
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            Tags { "Projection" = "Cylindrical" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_cylindrical

            float frag_cylindrical(v2f i) : SV_Target
            {
                return GetColor(GetPointCylindricalFromUV(i.uv));
            }
            ENDCG
        }
    }
}
