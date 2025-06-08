Shader "Xnoise/Transformers/Turbulence"
{
    Properties
    {
        _OriginalDisplacementMap("Original Displacement Map", 2D) = "black" {}
        _PerlinA("PerlinA", 2D) = "black" {}
        _PerlinB("PerlinB", 2D) = "black" {}
        _PerlinC("PerlinC", 2D) = "black" {}
        _Power("Power", Float) = 1
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

            float _Power;
            sampler2D _OriginalDisplacementMap;
            sampler2D _PerlinA;
            float4 _PerlinA_ST;
            sampler2D _PerlinB;
            float4 _PerlinB_ST;
            sampler2D _PerlinC;
            float4 _PerlinC_ST;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float xd = tex2Dlod(_PerlinA, float4(i.uv, 0, 0));
                float yd = tex2Dlod(_PerlinB, float4(i.uv, 0, 0));
                float zd = tex2Dlod(_PerlinC, float4(i.uv, 0, 0));
                float4 originalDisplacement = tex2Dlod(_OriginalDisplacementMap, float4(i.uv, 0, 0));

                return originalDisplacement + ((float4(xd, yd, zd, 1) * _Power) / UNIT_SCALE);
            }
            ENDCG
        }
    }
}
