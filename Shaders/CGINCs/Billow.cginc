#ifndef BILLOW_SHADER_LOGIC
#define BILLOW_SHADER_LOGIC

float GetBillow(float3 position, float seed, float frequency, float persistence, float lacunarity, int octaves)
{
    float value = 0.0;
    float signal = 0.0;
    float curPersistence = 1.0;
    float nx, ny, nz;

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
        //seed = (_Seed + curOctave) & 0xffffffff;
        //signal = GradientCoherentNoise3D(nx, ny, nz, seed, 1);
        signal = snoise(float3(nx + seed, ny + seed, nz + seed));
        signal = 2.0 * abs(signal) - 1.0;
        value += signal * curPersistence;

        // Prepare the next octave.
        position.x *= lacunarity;
        position.y *= lacunarity;
        position.z *= lacunarity;
        curPersistence *= persistence;
    }

    value += 0.5;

    return value;
}

#endif