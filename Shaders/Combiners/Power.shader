Shader "Xnoise/Combiners/Power"
{
    // This is a simple shader that contain the minimum to be used in Xnoise
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
            sampler2D _TextureB;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed frag(v2f i) : SV_Target
            {
                return pow(tex2Dlod(_TextureA, float4(i.uv, 0, 0)).x, tex2Dlod(_TextureB, float4(i.uv, 0, 0)).x);
            }
            ENDCG
        }
    }
}
