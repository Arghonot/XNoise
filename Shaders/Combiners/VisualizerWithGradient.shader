Shader "Xnoise/VisualizerWithGradient"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "black" {}
        _Gradient("Gradient", 2D) = "black" {}
    }
    SubShader
    {
        Cull Off
        ZWrite On
        ZTest Lequal

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

            sampler2D _MainTex;
            sampler2D _Gradient;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float color = tex2D(_MainTex, i.uv).x;
                float4 finalColor = tex2D(_Gradient, float2(color, 0.5));
                return float4(finalColor.xyz, 1);
            }
            ENDCG
        }
    }
}
