Shader "Xnoise/Combiners/Subtract"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
        _TextureB("TextureB", 2D) = "white" {} }
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
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TextureA;
            float4 _TextureA_ST;
            sampler2D _TextureB;
            float4 _TextureB_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _TextureA);
                o.uv2 = TRANSFORM_TEX(v.uv2, _TextureB);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float a = (tex2D(_TextureA, i.uv1) * 2) - 1;
                float b = (tex2D(_TextureB, i.uv2) * 2) - 1;
                float color = ((b - a) + 1) / 2;

                //float color = (tex2D(_TextureB, i.uv2) - tex2D(_TextureA, i.uv1));

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
