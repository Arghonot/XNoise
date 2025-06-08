Shader "Xnoise/Transformers/Displace"
{
    Properties
    {
        _OriginalDisplacementMap("Original Displacement Map", 2D) = "black" {}
        _TextureA("TextureA", 2D) = "black" {}
        _TextureB("TextureB", 2D) = "black" {}
        _TextureC("TextureC", 2D) = "black" {}
        _Influence("Influence", Float) = 1
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
                #include "../CGINCs/LibnoiseUtils.cginc"
                #include "../CGINCs/XnoiseCommon.cginc"

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

                sampler2D _OriginalDisplacementMap;
                float4 _OriginalDisplacementMap_ST;
                sampler2D _TextureA;
                float4 _TextureA_ST;
                sampler2D _TextureB;
                float4 _TextureB_ST;
                sampler2D _TextureC;
                float4 _TextureC_ST;
                float _Influence;

                v2f vert(appdata v)
                {
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _TextureA);

                    return o;
                }

                float getColorGrayscale(float3 sample)
                {
                    return 0.21 * sample.r + 0.71 * sample.g + 0.07 * sample.b;
                }

                float4 GetDisplace(float2 uv1)
                {
                    float x = getColorGrayscale(tex2D(_TextureA, uv1));
                    float y = getColorGrayscale(tex2D(_TextureB, uv1));
                    float z = getColorGrayscale(tex2D(_TextureC, uv1));

                    return float4(x, y, z, 1);
                }

                float4 frag(v2f i) : SV_Target
                {
                    float4 color = (GetDisplace(i.uv) * _Influence) / UNIT_SCALE;
                    float4 originalColor = tex2D(_OriginalDisplacementMap, i.uv);

                    return originalColor + color;
                }
                ENDCG
            }
        }
}
