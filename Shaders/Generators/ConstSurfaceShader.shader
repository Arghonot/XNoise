﻿Shader "Xnoise/Generators/ConstSurfaceShader"
{
    Properties
    {
        _Const("Const", Float) = 1
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Tags { "RenderType" = "Opaque" }
        LOD 100
        CGINCLUDE
        #include "UnityCG.cginc"

        float _Const;

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
                return _Const;
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
                return _Const;
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
                return _Const;
            }
            ENDCG
        }
    }
}
