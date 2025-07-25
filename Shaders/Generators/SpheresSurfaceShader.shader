﻿Shader "Xnoise/Generators/SpheresSurfaceShader"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Radius("radius",Float) = 1.0
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("rotation", Vector) = (0, 0, 0, 1)
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        
        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

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

        float _Frequency;

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;

            return o;
        }

        float GetSpheres(float x, float y, float z)
        {
            x *= _Frequency;
            y *= _Frequency;
            z *= _Frequency;
            float dfc = sqrt(x * x + y * y + z * z);
            float dfss = dfc - floor(dfc);
            float dfls = 1.0 - dfss;
            float nd = min(dfss, dfls);

            return 1.0 - (nd * 4.0);
        }

        ENDCG
        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Planar

            float frag_Planar(v2f i) : SV_Target
            {
                float3 coord = GetPlanarCartesianFromUV(i.uv, _Radius);
                return Normalize(GetSpheres(coord.x, coord.y, coord.z));
            }
            ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Spherical

            float frag_Spherical(v2f i) : SV_Target
            {
                float3 coord = GetSphericalCartesianFromUV(i.uv, _Radius);
                if (length(coord) <= 2) coord += float3(10.0, 0.0, 0.0);
                return Normalize(GetSpheres(coord.x, coord.y, coord.z));
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Cylindrical

            float frag_Cylindrical(v2f i) : SV_Target
            {
                float3 coord = GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius);
                return Normalize(GetSpheres(coord.x, coord.y, coord.z));
            }
            ENDCG
        }
    }
}
