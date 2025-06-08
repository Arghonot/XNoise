Shader "Xnoise/Modifiers/Clamp"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
        _Minimum("Minimum", Float) = 0
        _Maximum("Maximum", Float) = 1
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

            float _Minimum, _Maximum;
            sampler2D _TextureA;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float GetClamp(float value)
            {
                if (value < _Minimum) return _Minimum;
                if (value > _Maximum) return _Maximum;
                return value;
            }

            float frag(v2f i) : SV_Target
            {
                return GetClamp(tex2Dlod(_TextureA, float4(i.uv, 0, 0)).x);
            }
            ENDCG
        }
    }
}
