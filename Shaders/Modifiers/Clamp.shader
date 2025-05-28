Shader "Xnoise/Modifiers/Clamp"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
        _Minimum("Minimum", Float) = 0
        _Maximum("Maximum", Float) = 1
    }
        SubShader
    {
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

            float _Minimum, _Maximum;
            sampler2D _TextureA;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TextureA);

                return o;
            }


            float GetClamp(float value)
            {
                return clamp(value * 2 - 1, _Minimum, _Maximum);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float color = GetClamp(tex2D(_TextureA, i.uv));

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
