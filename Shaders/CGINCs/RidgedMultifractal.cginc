#ifndef RIDGEDMULTIFRACTAL_SHADER_LOGIC
#define RIDGEDMULTIFRACTAL_SHADER_LOGIC


float GetRidgedMultifractal(float3 position, float frequency, float lacunarity, int octaves)
{
    float value = 0.0;
    float signal = 0.0;
    float weight = 1.0;
    float offset = 1.0;
    float gain = 2.0;
    float _weights[12];
    float nx, ny, nz;
    float f = 1.0;

    for (int i = 0; i < octaves; i++)
    {
        _weights[i] = pow(f, -1);
        f *= lacunarity;
    }

    position.x *= frequency;
    position.y *= frequency;
    position.z *= frequency;

    for (int curOctave = 0; curOctave < octaves; curOctave++) {

        // Make sure that these floating-point values have the same range as a 32-
        // bit integer so that we can pass them to the coherent-noise functions.
        nx = position.x;
        ny = position.y;
        nz = position.z;

        // Get the coherent-noise value from the input value and add it to the
        // final result.
        //seed = (m_seed + curOctave) & 0xffffffff;
        //signal = GradientCoherentNoise3D(nx, ny, nz, seed, 1);
        signal = snoise(float3(nx, ny, nz));
        signal = abs(signal);
        signal = offset - signal;
        signal *= signal;
        signal *= weight;

        weight = signal * 2.0;
        weight = clamp(weight, 0, 1);
        value += (signal * _weights[i]);
        value = signal;

        position.x *= lacunarity;
        position.y *= lacunarity;
        position.z *= lacunarity;
    }

    return (value * 1.25) - 1.0;
}

#endif