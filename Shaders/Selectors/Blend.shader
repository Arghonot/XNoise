Shader "Xnoise/Selectors/Blend"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
        _TextureB("TextureB", 2D) = "black" {}
        _Controller("Controller", 2D) = "black" {}
    }
    SubShader
    {
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

            sampler2D _TextureA;
            sampler2D _TextureB;
            sampler2D _Controller;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float color = lerp(tex2Dlod(_TextureA, float4(i.uv, 0, 0)), tex2Dlod(_TextureB, float4(i.uv, 0, 0)), tex2Dlod(_Controller, float4(i.uv, 0, 0)));

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
