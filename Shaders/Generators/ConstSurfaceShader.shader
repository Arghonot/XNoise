Shader "Xnoise/Generators/ConstSurfaceShader"
{
    Properties
    {
        _Const("Const", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"

        float _Const;

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
        ENDCG

        Pass
        {
            Name "PLANAR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                float color = (_Const);

                return float4(color, color, color, 1);
            }
            ENDCG
        }
                Pass
        {
            Name "SPHERICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                float color = (_Const);

                return float4(color, color, color, 1);
            }
            ENDCG
        }
                Pass
        {
            Name "CYLINDRICAL"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                float color = (_Const);

                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}
