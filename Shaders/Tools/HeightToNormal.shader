Shader "Hidden/Xnoise/Tools/HeightToNormal"
{
    Properties
    {
        _HeightMap ("Height Map", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 1.0
        _TexelSize ("Texel Size", Vector) = (1, 1, 0, 0)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _HeightMap;
            float4 _HeightMap_ST;
            float _Intensity;
            float4 _TexelSize; // x = width, y = height in pixels

            // Sample height map with proper boundary handling
            float SampleHeight(float2 uv, float2 offset)
            {
                // Calculate pixel coordinates
                float2 pixelCoord = uv * _TexelSize.xy + offset;
                
                // Clamp to texture boundaries (equivalent to your Mathf.Max/Min logic)
                pixelCoord.x = clamp(pixelCoord.x, 0, _TexelSize.x - 1);
                pixelCoord.y = clamp(pixelCoord.y, 0, _TexelSize.y - 1);
                
                // Convert back to UV coordinates
                float2 clampedUV = pixelCoord / _TexelSize.xy;
                
                // Sample the height map (assuming grayscale, using red channel)
                return tex2D(_HeightMap, clampedUV).r;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _HeightMap);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate texel size in UV space
                float2 texelSize = 1.0 / _TexelSize.xy;
                
                // Sample neighboring heights (equivalent to your border sampling)
                // Using 1 pixel offset (equivalent to _ucBorder = 1)
                float heightLeft = SampleHeight(i.uv, float2(-1, 0));
                float heightRight = SampleHeight(i.uv, float2(1, 0));
                float heightDown = SampleHeight(i.uv, float2(0, -1));
                float heightUp = SampleHeight(i.uv, float2(0, 1));
                
                // Calculate gradients (matching your algorithm)
                // xPos = (left - right) / 2
                float xPos = (heightLeft - heightRight) * 0.5;
                // yPos = (down - up) / 2  
                float yPos = (heightDown - heightUp) * 0.5;
                
                // Create normal vectors (matching your Vector3 construction)
                float3 normalX = float3(xPos * _Intensity, 0, 1);
                float3 normalY = float3(0, yPos * _Intensity, 1);
                
                // Combine and normalize the normal vector
                float3 normalVector = normalX + normalY;
                normalVector = normalize(normalVector);
                
                // Convert normal from [-1,1] to [0,1] range for color output
                float3 colorVector = (normalVector + 1.0) * 0.5;
                
                //return fixed4(1.0, 1.0, 1.0, 1.0);
                return fixed4(colorVector.x, colorVector.y, colorVector.z, 1.0);
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}