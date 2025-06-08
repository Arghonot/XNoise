Shader "Xnoise/Combiners/Add"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
        _TextureB("TextureB", 2D) = "black" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            sampler2D _TextureB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return (tex2Dlod(_TextureA, float4(i.uv, 0, 0)) + tex2Dlod(_TextureB, float4(i.uv, 0, 0))) / 2.0;
            }
            ENDCG
        }
    }
}
