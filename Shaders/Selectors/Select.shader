Shader "Xnoise/Selectors/Select"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
        _TextureB("TextureB", 2D) = "white" {}
        _TextureC("TextureC", 2D) = "white" {}
        _FallOff("Falloff", Float) = 1

        _SelectLowerBound("Select Lower Bound", Float) = 1
        _SelectUpperBound("Select Upper Bound", Float) = 1
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
            float _SelectLowerBound, _SelectUpperBound;
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
                    //return t * t * (3.0 - 2.0 * t);

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

            float GetValueSelect2(float2 uv)
            {
                float valueC = tex2D(_TextureC, uv).r;
                float valueA = tex2D(_TextureA, uv).r;
                float valueB = tex2D(_TextureB, uv).r;

                // Optional: expose these as uniform floats
                float lowerBound = _SelectLowerBound;
                float upperBound = _SelectUpperBound;
                float edgeFalloff = _FallOff;

                // Clamp edgeFalloff to valid range
                float boundSize = upperBound - lowerBound;
                edgeFalloff = clamp(edgeFalloff, 0.0, boundSize / 2.0);

                // Inside the bounds: return TextureB
                if (edgeFalloff > 0.0)
                {
                    if (valueC < (lowerBound - edgeFalloff))
                    {
                        return valueA;
                    }
                    else if (valueC < (lowerBound + edgeFalloff))
                    {
                        float t = (valueC - (lowerBound - edgeFalloff)) / (2.0 * edgeFalloff);
                        return lerp(valueA, valueB, t);
                    }
                    else if (valueC < (upperBound - edgeFalloff))
                    {
                        return valueB;
                    }
                    else if (valueC < (upperBound + edgeFalloff))
                    {
                        float t = (valueC - (upperBound - edgeFalloff)) / (2.0 * edgeFalloff);
                        return lerp(valueB, valueA, t);
                    }
                    else
                    {
                        return valueA;
                    }
                }
                else
                {
                    return (valueC >= lowerBound && valueC <= upperBound) ? valueB : valueA;
                }
            }

            /*float MapCubicSCurve(float t)
            {
                // Equivalent to libnoise's cubic S-curve: 3t² - 2t³
                return t * t * (3.0 - 2.0 * t);
            }*/

            float GetValueSelect3(float2 uv)
            {
                float cv = tex2D(_TextureC, uv).r;
                float va = tex2D(_TextureA, uv).r;
                float vb = tex2D(_TextureB, uv).r;

                float minVal = _SelectLowerBound;
                float maxVal = _SelectUpperBound;
                float falloff = _FallOff;

                if (falloff > 0.0)
                {
                    float a;

                    if (cv < (minVal - falloff))
                    {
                        return va;
                    }
                    if (cv < (minVal + falloff))
                    {
                        float lc = minVal - falloff;
                        float uc = minVal + falloff;
                        float t = (cv - lc) / (uc - lc);
                        a = MapCubicSCurve(t);
                        return lerp(va, vb, a);
                    }
                    if (cv < (maxVal - falloff))
                    {
                        return vb;
                    }
                    if (cv < (maxVal + falloff))
                    {
                        float lc = maxVal - falloff;
                        float uc = maxVal + falloff;
                        float t = (cv - lc) / (uc - lc);
                        a = MapCubicSCurve(t);
                        return lerp(vb, va, a);
                    }

                    return va;
                }

                if (cv < minVal || cv > maxVal)
                {
                    return va;
                }

                return vb;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float color = GetValueSelect3(
                        i.uv1);

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
