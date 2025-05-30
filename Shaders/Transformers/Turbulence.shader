Shader "Xnoise/Transformers/Turbulence"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
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

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv1 : TEXCOORD0;
                    float2 uv2 : TEXCOORD1;
                    float2 uv3 : TEXCOORD2;
                    float2 uv4 : TEXCOORD3;
        };

                struct v2f
                {
                    float2 uv1 : TEXCOORD0;
                    float2 uv2 : TEXCOORD1;
                    float2 uv3 : TEXCOORD2;
                    float2 uv4 : TEXCOORD4;
                    float4 vertex : SV_POSITION;
                };
                float X0;
                float Y0;
                float Z0;
                float X1;
                float Y1;
                float Z1;
                float X2;
                float Y2;
                float Z2;
                float _Power;
                sampler2D _TextureA;
                float4 _TextureA_ST;
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
                    o.uv1 = TRANSFORM_TEX(v.uv1, _TextureA);
                    o.uv2 = TRANSFORM_TEX(v.uv2, _PerlinA);
                    o.uv3 = TRANSFORM_TEX(v.uv3, _PerlinB);
                    o.uv4 = TRANSFORM_TEX(v.uv4, _PerlinC);
                    X0 = (12414.0 / 65536.0);
                    Y0 = (65124.0 / 65536.0);
                    Z0 = (31337.0 / 65536.0);
                    X1 = (26519.0 / 65536.0);
                    Y1 = (18128.0 / 65536.0);
                    Z1 = (60493.0 / 65536.0);
                    X2 = (53820.0 / 65536.0);
                    Y2 = (11213.0 / 65536.0);
                    Z2 = (44845.0 / 65536.0);

                    return o;
                }

                float2 GetUVFromPosition(float x, float y, float z)
                {
                    float2 lnlat = GetSphericalFromCartesian(x, y, z);
                    return GetUVFromSpherical(lnlat.x, lnlat.y);
                }

                float GetTurbulence(float2 uv1, float2 uv2, float2 uv3, float x, float y, float z)
                {
                    float2 uvA = GetUVFromPosition(x + X0, y + Y0, z + Z0);
                    float2 uvB = GetUVFromPosition(x + X0, y + Y0, z + Z0);
                    float2 uvC = GetUVFromPosition(x + X0, y + Y0, z + Z0);

                    float xd = x + (tex2D(_PerlinA, uvA) * _Power);
                    float yd = y + (tex2D(_PerlinB, uvB) * _Power);
                    float zd = z + (tex2D(_PerlinC, uvC) * _Power);

                    return tex2D(_TextureA, GetUVFromPosition(xd, yd, zd));
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float3 pos = GetSphericalCartesianFromUV(i.uv1.x, i.uv1.y, 1);

                    float color = GetTurbulence(
                            i.uv1,
                            i.uv2,
                            i.uv3,
                            pos.x,
                            pos.y,
                            pos.z);

                    return float4(color, color, color, 1);
                }
                ENDCG
            }
        }
}
