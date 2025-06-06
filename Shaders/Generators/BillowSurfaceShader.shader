Shader "Xnoise/Generators/BillowSurfaceShader"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Lacunarity("Lacunarity", Float) = 1
        _Persistence("Persistence", Float) = 1
        _Octaves("Octaves", Float) = 1
        _Radius("Radius", Float) = 1.0
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("Rotation", Vector) = (0, 0, 0, 1)
        _Seed("Seed", Float) = 1
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/noiseSimplex.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/Billow.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

        float _Seed, _Frequency, _Lacunarity, _Octaves, _Persistence;

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

        float4 BillowFragment(float3 pos, float2 uv)
        {
            float3 transformedPos = ApplyTransformOperations(pos, uv);
            float color = GetBillow(transformedPos, _Frequency, _Persistence, _Lacunarity, _Octaves);

            color = Normalize(color);
            return float4(color, color, color, 1);
        }
        ENDCG

        // PLANAR
        Pass
        {
            Name "PLANAR"
            Tags { "Projection" = "Planar" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                return BillowFragment(GetPlanarCartesianFromUV(i.uv, _OffsetPosition.xyz), i.uv);
            }
            ENDCG
        }

        // SPHERICAL
        Pass
        {
            Name "SPHERICAL"
            Tags { "Projection" = "Spherical" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                return BillowFragment(GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius), i.uv);
            }
            ENDCG
        }

        // CYLINDRICAL
        Pass
        {
            Name "CYLINDRICAL"
            Tags { "Projection" = "Cylindrical" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(v2f i) : SV_Target
            {
                return BillowFragment(GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius), i.uv);
            }
            ENDCG
        }
    }
}
