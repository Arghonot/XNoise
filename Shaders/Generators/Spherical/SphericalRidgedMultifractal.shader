Shader "Xnoise/Generators/RidgedMultifractalSpherical"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Lacunarity("Lacunarity", Float) = 1
        _Octaves("Octaves", Float) = 1
        _Radius("Radius",Float) = 1
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Seed("Seed",Int) = 42
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
            #include "../../noiseSimplex.cginc"
            #include "../../LibnoiseUtils.cginc"
            #include "../../CGINCs/RidgedMultifractal.cginc"

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
            float _Seed, _Frequency, _Lacunarity, _Octaves;
            int _Radius;
            float4 _OffsetPosition;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 pos = GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius);

                pos = float3(pos.x + _OffsetPosition.x, pos.y + _OffsetPosition.y, pos.z + _OffsetPosition.z);

                float color = GetRidgedMultifractal(pos, _Frequency, _Lacunarity, _Octaves) / 2 + 0.5f;

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
