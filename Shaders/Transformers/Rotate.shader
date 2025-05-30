Shader "Xnoise/Transformers/Rotate"
{
    Properties
    {
        _TextureA("TextureA", 2D) = "white" {}
        _X("X", Float) = 1
        _Y("Y", Float) = 1
        _Z("Z", Float) = 1
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

                float _X, _Y, _Z;
                sampler2D _TextureA;
                float4 _TextureA_ST;

                v2f vert(appdata v)
                {
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv1 = TRANSFORM_TEX(v.uv1, _TextureA);

                    return o;
                }

                float GetRotate(float x, float y, float z)
                {
                    float xCos, yCos, zCos, xSin, ySin, zSin;

                    xCos = cos(_X * 0.01745329251);
                    yCos = cos(_Y * 0.01745329251);
                    zCos = cos(_Z * 0.01745329251);
                    xSin = sin(_X * 0.01745329251);
                    ySin = sin(_Y * 0.01745329251);
                    zSin = sin(_Z * 0.01745329251);

                    float m_x1Matrix = ySin * xSin * zSin + yCos * zCos;
                    float m_y1Matrix = xCos * zSin;
                    float m_z1Matrix = ySin * zCos - yCos * xSin * zSin;
                    float m_x2Matrix = ySin * xSin * zCos - yCos * zSin;
                    float m_y2Matrix = xCos * zCos;
                    float m_z2Matrix = -yCos * xSin * zCos - ySin * zSin;
                    float m_x3Matrix = -ySin * xCos;
                    float m_y3Matrix = xSin;
                    float m_z3Matrix = yCos * xCos;

                    float nx = (m_x1Matrix * x) + (m_y1Matrix * y) + (m_z1Matrix * z);
                    float ny = (m_x2Matrix * x) + (m_y2Matrix * y) + (m_z2Matrix * z);
                    float nz = (m_x3Matrix * x) + (m_y3Matrix * y) + (m_z3Matrix * z);
                    float3 normalized = normalize(float3(nx, ny, nz));
                    float2 FinalLnLat = GetSphericalFromCartesian(normalized.x, normalized.y, normalized.z);
                    float2 FinalUV = GetUVFromSpherical(FinalLnLat.x, FinalLnLat.y);

                    //float nx = x + _X;
                    //float ny = y + _Y;
                    //float nz = z + _Z;

                    //float3 normalized = normalize(float3(nx, ny, nz));
                    //float2 FinalLnLat = GetSphericalFromCartesian(normalized.x, normalized.y, normalized.z);
                    //float2 FinalUV = GetUVFromSpherical(FinalLnLat.x, FinalLnLat.y);

                    return tex2D(_TextureA, FinalUV);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float3 pos = GetSphericalCartesianFromUV(i.uv1.x, i.uv1.y, 1);

                    float color = GetRotate(
                            pos.x,
                            pos.y,
                            pos.z);

                    return float4(color, color, color, 1);
                }
                ENDCG
            }
        }
}
