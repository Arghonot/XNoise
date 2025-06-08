Shader "Xnoise/Modifiers/Invert"
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
                return 1 - value;
            }

            float4 frag(v2f i) : SV_Target
            {
                float color = GetValueInvert(tex2Dlod(_TextureA, float4(i.uv, 0, 0)));

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
