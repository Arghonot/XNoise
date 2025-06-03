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
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/noiseSimplex.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/Billow.cginc"

        float _Seed, _Frequency, _Lacunarity, _Octaves, _Persistence, _Radius;
        float4 _OffsetPosition, _Rotation;

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

        float4 BillowFragment(float3 pos)
        {
            float3 rotated = GetRotatedPositions(pos, _OffsetPosition, _Rotation);
            float color = (GetBillow(rotated, _Frequency, _Persistence, _Lacunarity, _Octaves) + 1) * 0.5;
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
                float3 pos = GetPlanarCartesianFromUV(i.uv, _OffsetPosition.xyz);
                return BillowFragment(pos);
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
                float3 pos = GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                return BillowFragment(pos);
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
                float3 pos = GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius);
                return BillowFragment(pos);
            }
            ENDCG
        }
    }
}
