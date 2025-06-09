Shader "Xnoise/Modifiers/Terrace"
{
    Properties
    {
        _Src("Source", 2D) = "black" {}
        _Gradient("Gradient", 2D) = "black" {}
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Tags { "RenderType"="Opaque" }
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

            sampler2D _Src;
            float4 _Src_ST;
            sampler2D _Gradient;
            float4 _Gradient_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float frag (v2f i) : SV_Target
            {
                float color = clamp(tex2D(_Src, i.uv), 0, 0.975);
                return tex2D(_Gradient, float2(color, i.uv.y)).x;
            }
            ENDCG
        }
    }
}
