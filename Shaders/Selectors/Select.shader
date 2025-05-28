Shader "Xnoise/Selectors/Select"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
        _TextureB("TextureB", 2D) = "white" {}
        _TextureC("TextureC", 2D) = "white" {}
        _FallOff("Falloff", Float) = 1
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
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            float _FallOff, _raw, _min, _max;
            sampler2D _TextureA;
            float4 _TextureA_ST;
            sampler2D _TextureB;
            float4 _TextureB_ST;
            sampler2D _TextureC;
            float4 _TextureC_ST;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _TextureA);
                o.uv2 = TRANSFORM_TEX(v.uv2, _TextureB);
                o.uv3 = TRANSFORM_TEX(v.uv3, _TextureC);

                return o;
            }


            float InterpolateLinear(float a, float b, float position)
            {
                return ((1.0 - position) * a) + (position * b);
            }

            float MapCubicSCurve(float value)
            {
                return (value * value * (3.0 - 2.0 * value));
            }

            float GetValueSelect(float2 uv1, float2 uv2, float2 uv3)
            {
                _min = -1.0;
                _max = 1.0;
                float cv = tex2D(_TextureC, uv3);

                cv = cv * 2 - 1;

                if (_FallOff > 0.0)
                {
                    float a;
                    if (cv < (_min - _FallOff))
                    {
                        return tex2D(_TextureA, uv1);
                    }
                    if (cv < (_min + _FallOff))
                    {
                        float lc = (_min - _FallOff);
                        float uc = (_min + _FallOff);
                        a = MapCubicSCurve((cv - lc) / (uc - lc));
                        return InterpolateLinear(
                            tex2D(_TextureA, uv1),
                            tex2D(_TextureB, uv2),
                            a);
                    }
                    if (cv < (_max - _FallOff))
                    {
                        return tex2D(_TextureB, uv2);
                    }
                    if (cv < (_max + _FallOff))
                    {
                        float lc = (_max - _FallOff);
                        float uc = (_max + _FallOff);
                        a = MapCubicSCurve((cv - lc) / (uc - lc));
                        return InterpolateLinear(
                            tex2D(_TextureB, uv2),
                            tex2D(_TextureA, uv1), a);
                    }
                    return tex2D(_TextureA, uv1);
                }
                if (cv < _min || cv > _max)
                {
                    return 1.0;
                    return tex2D(_TextureA, uv1);
                }
                return tex2D(_TextureB, uv2);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float color = GetValueSelect(
                        i.uv1,
                        i.uv2,
                        i.uv3);

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
