Shader "Xnoise/Transformers/Turbulence"
{
    Properties
    {
        _PerlinA("PerlinA", 2D) = "white" {}
        _PerlinB("PerlinB", 2D) = "white" {}
        _PerlinC("PerlinC", 2D) = "white" {}
        _Power("Power", Float) = 1
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
                    float2 uv1 : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv1 : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float _Power;
                sampler2D _PerlinA;
                float4 _PerlinA_ST;
                sampler2D _PerlinB;
                float4 _PerlinB_ST;
                sampler2D _PerlinC;
                float4 _PerlinC_ST;

                v2f vert(appdata v)
                {
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv1 = TRANSFORM_TEX(v.uv1, _PerlinA);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float xd = tex2D(_PerlinA, i.uv1);
                    float yd = tex2D(_PerlinB, i.uv1);
                    float zd = tex2D(_PerlinC, i.uv1);

                    return fixed4(xd, yd, zd, _Power / UNIT_SCALE);
                }
                ENDCG
            }
        }
}
