Shader "Xnoise/Generators/SphericalCylinder"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
        _Radius("radius",Float) = 1.0
        _OffsetPosition("Offset", Vector) = (0,0,0,0)
        _Rotation("rotation", Vector) = (0, 0, 0, 1)
        _DisplacementMap("DisplacementMap", 2D) = "white" {}
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

            float _Frequency, _Lacunarity, _Octaves, _Persistence;
            int _Radius;
            float4 _OffsetPosition, _Rotation;
            sampler2D _DisplacementMap;
            float4 _DisplacementMap_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            float ComputeCylinder(float x, float y, float z)
            {
                x *= _Frequency;
                z *= _Frequency;
                float dfc = sqrt(x * x + z * z);
                float dfss = dfc - floor(dfc);
                float dfls = /*1.0 - */dfss;
                float nd = min(dfss, dfls);

                return 1.0 - (nd * 4.0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 pos = GetSphericalCartesianFromUV(i.uv.x, i.uv.y, _Radius);
                
                pos = GetRotatedPositions(pos, _OffsetPosition, _Rotation) + tex2D(_DisplacementMap, i.uv);

                float color = ComputeCylinder(
                    pos.x + _OffsetPosition.x,
                    pos.y + _OffsetPosition.y,
                    pos.z + _OffsetPosition.z) / 2 + 0.5f;

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
