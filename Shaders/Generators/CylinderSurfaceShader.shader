Shader "Xnoise/Generators/CylinderSurfaceShader"
{
    Properties
    {
        _Frequency("Frequency", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "../CGINCs/LibnoiseUtils.cginc"
        #include "../CGINCs/XnoiseCommon.cginc"

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

        float _Frequency;

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

        ENDCG
        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Planar

            float frag_Planar(v2f i) : SV_Target
            {
                float3 coord = GetPointPlanarFromUV(i.uv);
                return Normalize(ComputeCylinder(coord.x, coord.y, coord.z));
            }
            ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Spherical

            float frag_Spherical(v2f i) : SV_Target
            {
                float3 coord = GetPointSphericalFromUV(i.uv);
                if (length(coord) <= 2) coord += float3(10.0, 0.0, 0.0);
                return Normalize(ComputeCylinder(coord.x, coord.y, coord.z));
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Cylindrical

            float frag_Cylindrical(v2f i) : SV_Target
            {
                float3 coord = GetPointCylindricalFromUV(i.uv);
                return Normalize(ComputeCylinder(coord.x, coord.y, coord.z));
            }
            ENDCG
        }
    }
}
