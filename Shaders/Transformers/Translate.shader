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
                #include "../LibnoiseUtils.cginc"

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

                float GetTranslate(float x, float y, float z)
                {
                    float xc = cos(_X * 0.0174533);
                    float yc = cos(_Y * 0.0174533);
                    float zc = cos(_Z * 0.0174533);
                    float xs = sin(_X * 0.0174533);
                    float ys = sin(_Y * 0.0174533);
                    float zs = sin(_Z * 0.0174533);
                    float _x1Matrix = ys * xs * zs + yc * zc;
                    float _y1Matrix = xc * zs;
                    float _z1Matrix = ys * zc - yc * xs * zs;
                    float _x2Matrix = ys * xs * zc - yc * zs;
                    float _y2Matrix = xc * zc;
                    float _z2Matrix = -yc * xs * zc - ys * zs;
                    float _x3Matrix = -ys * xc;
                    float _y3Matrix = xs;
                    float _z3Matrix = yc * xc;

                    float nx = (_x1Matrix * x) + (_y1Matrix * y) + (_z1Matrix * z);
                    float ny = (_x2Matrix * x) + (_y2Matrix * y) + (_z2Matrix * z);
                    float nz = (_x3Matrix * x) + (_y3Matrix * y) + (_z3Matrix * z);

                    float3 normalized = normalize(float3(nx, ny, nz));
                    float2 FinalLnLat = GetSphericalFromCartesian(normalized.x, normalized.y, normalized.z);
                    float2 FinalUV = GetUVFromSpherical(FinalLnLat.x, FinalLnLat.y);

                    return tex2D(_TextureA, FinalUV);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float3 pos = GetSphericalCartesianFromUV(i.uv1.x, i.uv1.y, 1);

                    float val = GetTranslate(
                            pos.x,
                            pos.y,
                            pos.z);

                    return float4(val, val, val, val);
                }
                ENDCG
            }
        }
}
