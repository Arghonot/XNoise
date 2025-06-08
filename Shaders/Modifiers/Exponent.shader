Shader "Xnoise/Modifiers/Exponent"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
        _Exponent("Exponent", Float) = 1.0
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
            float _Exponent;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float GetValueExponent(float value)
            {
                return pow(value, _Exponent);
            }

            float4 frag(v2f i) : SV_Target
            {
                float color = GetValueExponent(tex2Dlod(_TextureA, float4(i.uv, 0, 0)));

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
