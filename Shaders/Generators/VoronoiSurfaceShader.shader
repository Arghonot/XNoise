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
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
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

        sampler2D _Permutations;
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

        float4 GetColor(float3 coord)
        {
            _Radius = _Frequency;
            float val = VoronoiGetValue(coord, _Seed, _Frequency, _Distance, _Displacement);
            float normalized = (val + 1) * 0.5;

            return float4(normalized, normalized, normalized, 1);
        }
        ENDCG

        Pass
        {
            Name "PLANAR"
            Tags { "Projection" = "Planar" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_planar

            float4 frag_planar(v2f i) : SV_Target
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

            float4 frag_spherical(v2f i) : SV_Target
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

            float4 frag_cylindrical(v2f i) : SV_Target
            {
                return GetColor(GetPointCylindricalFromUV(i.uv));
            }
            ENDCG
        }
    }
}
