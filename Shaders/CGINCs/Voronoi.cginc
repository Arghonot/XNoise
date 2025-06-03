#ifndef VORONOI_SHADER_LOGIC
#define VORONOI_SHADER_LOGIC

int GeneratorNoiseX = 1619;
int GeneratorNoiseY = 31337;
int GeneratorNoiseZ = 6971;
int GeneratorSeed = 1013;
int GeneratorShift = 8;

float ValueNoise3DInt(int x, int y, int z, int seed)
{
    int GeneratorNoiseX = 1619;
    int GeneratorNoiseY = 31337;
    int GeneratorNoiseZ = 6971;
    int GeneratorSeed = 1013;

    int n = (GeneratorNoiseX * x + GeneratorNoiseY * y + GeneratorNoiseZ * z +
        GeneratorSeed * seed) & 0x7fffffff;
    n = (n >> 13) ^ n;
    return (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
}

float ValueNoise3D(int x, int y, int z, int seed)
{
    return 1.0 - (ValueNoise3DInt(x, y, z, seed) / 1073741824.0);
}

float VoronoiGetValue(float3 position, float seed, float frequency, float distance, float displacement)
{
    float x = position.x;
    float y = position.y;
    float z = position.z;
    
    x *= frequency;
    y *= frequency;
    z *= frequency;

    int xInt = (x > 0.0 ? (int)x : (int)x - 1);
    int yInt = (y > 0.0 ? (int)y : (int)y - 1);
    int zInt = (z > 0.0 ? (int)z : (int)z - 1);

    float minDist = 2147483647.0;
    float xCandidate = 0;
    float yCandidate = 0;
    float zCandidate = 0;

    // Inside each unit cube, there is a seed point at a random position.  Go
    // through each of the nearby cubes until we find a cube with a seed point
    // that is closest to the specified position.
    for (int zCur = zInt - 2; zCur <= zInt + 2; zCur++) {
        for (int yCur = yInt - 2; yCur <= yInt + 2; yCur++) {
            for (int xCur = xInt - 2; xCur <= xInt + 2; xCur++) {

                // Calculate the position and distance to the seed point inside of
                // this unit cube.
                float xPos = xCur + ValueNoise3D(xCur, yCur, zCur, seed);
                float yPos = yCur + ValueNoise3D(xCur, yCur, zCur, seed + 1);
                float zPos = zCur + ValueNoise3D(xCur, yCur, zCur, seed + 2);
                float xDist = xPos - x;
                float yDist = yPos - y;
                float zDist = zPos - z;
                float dist = xDist * xDist + yDist * yDist + zDist * zDist;

                if (dist < minDist) {
                    // This seed point is closer to any others found so far, so record
                    // this seed point.
                    minDist = dist;
                    xCandidate = xPos;
                    yCandidate = yPos;
                    zCandidate = zPos;
                }
            }
        }
    }

    float value;
    if (distance > 0)
    {
        // Determine the distance to the nearest seed point.
        float xDist = xCandidate - x;
        float yDist = yCandidate - y;
        float zDist = zCandidate - z;
        value = 1 - (sqrt(xDist * xDist + yDist * yDist + zDist * zDist)
            ) * 1.73205080757;
    }
    else 
    {
        value = 0.0;
    }

    // Return the calculated distance with the displacement value applied.
    return value + (displacement * (float) ValueNoise3D(
        (int)(floor(xCandidate)),
        (int)(floor(yCandidate)),
        (int)(floor(zCandidate)),
        seed));
}

#endif