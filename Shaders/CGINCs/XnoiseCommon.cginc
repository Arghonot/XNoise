#ifndef XNOISE_COMMON_INCLUDED
#define XNOISE_COMMON_INCLUDED

#define UNIT_SCALE 10.0

float _Radius;
float3 _OffsetPosition;
float3 _Scale;
float4 _Rotation;
sampler2D _TurbulenceMap;
float4 _TurbulenceMap_ST;
float _TurbulencePower;

float3 ApplyTransformOperations(float3 p, float2 uv)
{
    // Apply Scale
    p *= _Scale;

    // Apply Translation
    p += _OffsetPosition;

    // Apply Rotation (quaternion)
    // q * v * q^-1
    float3 q = _Rotation.xyz;
    float qw = _Rotation.w;
    float3 t = 2.0 * cross(q, p);
    p = p + qw * t + cross(q, t);

    // Sample turbulence (centered around 0)
    float4 turbulence = tex2D(_TurbulenceMap, uv).xyzw;
    turbulence = (turbulence - 0.5) * 2.0;

    // Apply turbulence
    p = p + (turbulence.xyz * (turbulence.w * UNIT_SCALE));

    return p;
}

#endif