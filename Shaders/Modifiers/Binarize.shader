﻿Shader "Xnoise/Modifiers/Binarize"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
    }
        SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _TextureA;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float GetValueInvert(float value)
            {
                value = value * 2 - 1;

                if (abs(value - 1) > abs(value + 1))
                {
                    return 1;
                }

                return 0;
            }

            float frag(v2f i) : SV_Target
            {
                return GetValueInvert(tex2Dlod(_TextureA, float4(i.uv, 0, 0)).x);
            }
            ENDCG
        }
    }
}
