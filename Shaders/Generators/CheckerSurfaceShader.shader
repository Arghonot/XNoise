Shader "Xnoise/Generators/CheckerSurfaceShader"
{
    Properties
    {
        _Radius("radius",Float) = 1.0
        _TurbulenceMap("Turbulence Map", 2D) = "black" {}
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

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;

            return o;
        }

        float ComputeChecker(float x, float y, float z)
        {
            int ix = (int)(floor(x));
            int iy = (int)(floor(y));
            int iz = (int)(floor(z));

            return (ix & 1 ^ iy & 1 ^ iz & 1) != 0 ? -1.0 : 1.0;
        }

        ENDCG
        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Planar

            float4 frag_Planar(v2f i) : SV_Target
            {
                float3 coord = GetPlanarCartesianFromUV(i.uv, _OffsetPosition) * 2;
                float3 transformedPos = ApplyTransformOperations(coord, i.uv);
                float sphereValue = Normalize(ComputeChecker(coord.x, coord.y, coord.z));

                return float4(sphereValue, sphereValue, sphereValue, 1);
            }
            ENDCG
        }

        Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Spherical

            float4 frag_Spherical(v2f i) : SV_Target
            {
                float3 coord = GetSphericalCartesianFromUV(i.uv, _Radius);
                coord += float3(10.0, 0.0, 0.0);
                float3 transformedPos = ApplyTransformOperations(coord, i.uv);

                float sphereValue = Normalize(ComputeChecker(transformedPos.x, transformedPos.y, transformedPos.z));
                return float4(sphereValue, sphereValue, sphereValue, 1);
            }
            ENDCG
        }

        Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Cylindrical

            float4 frag_Cylindrical(v2f i) : SV_Target
            {
                float3 coord = GetCylindricalCartesianFromUV(i.uv, _OffsetPosition.xyz, _Radius);
                float sphereValue = Normalize(ComputeChecker(coord.x, coord.y, coord.z));
                return float4(sphereValue, sphereValue, sphereValue, 1);
            }
            ENDCG
        }
    }
}
