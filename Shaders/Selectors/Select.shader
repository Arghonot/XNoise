Shader "Xnoise/Selectors/Select"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "black" {}
        _TextureB("TextureB", 2D) = "black" {}
        _TextureC("TextureC", 2D) = "black" {}
        _FallOff("Falloff", Float) = 1
        _SelectLowerBound("Select Lower Bound", Float) = 1
        _SelectUpperBound("Select Upper Bound", Float) = 1
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

            float _FallOff, _raw, _min, _max;
            float _SelectLowerBound, _SelectUpperBound;
            sampler2D _TextureA;
            sampler2D _TextureB;
            sampler2D _TextureC;
            float4 _TextureA_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
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

            float GetValueSelect(float2 uv)
            {
                _min = -1.0;
                _max = 1.0;
                float cv = tex2Dlod(_TextureC, float4(uv, 0, 0));

                cv = cv * 2 - 1;

                if (_FallOff > 0.0)
                {
                    float a;
                    if (cv < (_min - _FallOff))
                    {
                        return tex2D(_TextureA, uv);
                    }
                    if (cv < (_min + _FallOff))
                    {
                        float lc = (_min - _FallOff);
                        float uc = (_min + _FallOff);
                        a = MapCubicSCurve((cv - lc) / (uc - lc));
                        return InterpolateLinear(
                            tex2D(_TextureA, uv),
                            tex2D(_TextureB, uv),
                            a);
                    }
                    if (cv < (_max - _FallOff))
                    {
                        return tex2D(_TextureB, uv);
                    }
                    if (cv < (_max + _FallOff))
                    {
                        float lc = (_max - _FallOff);
                        float uc = (_max + _FallOff);
                        a = MapCubicSCurve((cv - lc) / (uc - lc));
                        return InterpolateLinear(
                            tex2D(_TextureB, uv),
                            tex2D(_TextureA, uv), a);
                    }
                    return tex2D(_TextureA, uv);
                }
                if (cv < _min || cv > _max)
                {
                    return 1.0;
                    return tex2D(_TextureA, uv);
                }
                return tex2D(_TextureB, uv);
            }

            float GetValueSelect2(float2 uv)
            {
                float valueC = tex2D(_TextureC, uv).x;
                float valueA = tex2D(_TextureA, uv).x;
                float valueB = tex2D(_TextureB, uv).x;

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

            float GetValueSelect3(float2 uv)
            {
                float cv = tex2Dlod(_TextureC, float4(uv, 0, 0)).x;
                float va = tex2Dlod(_TextureA, float4(uv, 0, 0)).x;
                float vb = tex2Dlod(_TextureB, float4(uv, 0, 0)).x;

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


            float frag(v2f i) : SV_Target
            {
                return GetValueSelect3(i.uv);
            }
            ENDCG
        }
    }
}
