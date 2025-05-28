Shader "Xnoise/Generators/BillowPlanar"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Lacunarity("lacunarity", Float) = 1
        _Persistence("Persistence", Float) = 1
        _Octaves("Octaves", Float) = 1
        _Radius("radius",Float) = 1.0
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("rotation", Vector) = (0, 0, 0, 1)
        _Seed("seed", Float) = 1
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
            #include "../../CGINCs/Billow.cginc"

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

            float _Seed, _Frequency, _Lacunarity, _Octaves, _Persistence;
            int _Radius;
            float4 _OffsetPosition, _Rotation;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 pos = GetPlanarCartesianFromUV(i.uv, float3(_OffsetPosition.x,_OffsetPosition.y,_OffsetPosition.z));

                pos = GetRotatedPositions(pos, _OffsetPosition, _Rotation);

                float color = GetBillow(pos, _Frequency, _Persistence, _Lacunarity, _Octaves) / 2 + 0.5f;

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
