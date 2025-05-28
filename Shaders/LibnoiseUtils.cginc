#ifndef LIBNOISE_SHADER_UTILS
#define LIBNOISE_SHADER_UTILS

// from https://gist.github.com/patricknelson/f4dcaedda9eea5f5cf2c359f68aa35fd
float4 multQuat(float4 q1, float4 q2) {
    return float4(
    q1.w * q2.x + q1.x * q2.w + q1.z * q2.y - q1.y * q2.z,
    q1.w * q2.y + q1.y * q2.w + q1.x * q2.z - q1.z * q2.x,
    q1.w * q2.z + q1.z * q2.w + q1.y * q2.x - q1.x * q2.y,
    q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
    );
}
            
float3 rotateVector(float4 quat, float3 vec ) {
    // https://twistedpairdevelopment.wordpress.com/2013/02/11/rotating-a-vector-by-a-quaternion-in-glsl/
    float4 qv = multQuat( quat, float4(vec, 0.0) );
    return multQuat( qv, float4(-quat.x, -quat.y, -quat.z, quat.w) ).xyz;
}

// return a position using an origin(pos), an offset(offsets) and a rotation(rot)
float3 GetRotatedPositions(float3 pos, float3 offsets, float4 rot)
{
    return rotateVector(rot, float3(pos.x + offsets.x, pos.y + offsets.y, pos.z + offsets.z));
}

// --- to calculate 3d coordinates from uvs
float3 GetPlanarCartesianFromUV(float2 uv, float3 origin)
{
		return float3(origin.x + uv.x, origin.y + uv.y, origin.z);
}

float3 GetSphericalCartesianFromUV(float Ln, float Lat, float radius)
{
	Ln *= 360;
	Lat *= 180;
	Ln -= 180;
	Lat -= 90;
	Ln *= 0.017453292519943295;
	Lat *= 0.017453292519943295;

	return float3(
		radius * cos(Lat) * sin(Ln),
		radius * cos(Lat) * cos(Ln),
		radius * sin(Lat));
}

float2 GetSphericalFromCartesian(float x, float y, float z)
{
	float r = sqrt(pow(x, 2) + pow(y, 2) + pow(z, 2));

	float ln = atan2(x, y) % 3.14159;
	float lat = asin(z / r) % 1.5708;

	return float2(ln, lat);
}

float2 GetUVFromSpherical(float ln, float lat)
{
	float2 uv = float2(ln, lat);

	uv.x = ((uv.x / 0.017453292519943295) + 180) / 360;
	uv.y = ((uv.y / 0.017453292519943295) + 90) / 180;

	return uv;
}

#endif