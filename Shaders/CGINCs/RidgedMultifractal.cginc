#ifndef RIDGEDMULTIFRACTAL_SHADER_LOGIC
#define RIDGEDMULTIFRACTAL_SHADER_LOGIC
// Hash-based offset for each octave and seed
float3 RidgedSeedOffset(float seed, int octave)
{
    float x = frac(sin(seed + octave * 12.9898) * 43758.5453);
    float y = frac(sin(seed + octave * 78.233) * 12345.6789);
    float z = frac(sin(seed + octave * 45.164) * 98765.4321);
    return float3(x, y, z) * 256.0;
}

float GetRidgedMultifractal(float3 position, float seed, float frequency, float lacunarity, int octaves)
{
    // Ridged multifractal parameters
    const float offset = 1.0;
    const float gain = 2.0;

    float value = 0.0;
    float weight = 1.0;
    float amplitude = 1.0;
    float maxValue = 0.0;

    // Scale input position by frequency
    position *= frequency;

    for (int i = 0; i < octaves; i++)
    {
        // Offset the domain with a hash of seed and octave
        float3 octaveOffset = RidgedSeedOffset(seed, i);
        float3 p = position + octaveOffset;

        // Sample simplex noise
        float signal = snoise(p);

        // Ridged transformation
        signal = abs(signal);
        signal = offset - signal;
        signal *= signal;

        // Weighting
        signal *= weight;
        weight = clamp(signal * gain, 0.0, 1.0);

        // Accumulate
        value += signal * amplitude;
        maxValue += amplitude;

        // Prepare for next octave
        position *= lacunarity;
        amplitude *= 0.5; // Standard persistence for fractals
    }

    // Normalize and remap to [-1, 1]
    value = value / maxValue;
    return value * 2.0 - 1.0;
}

#endif