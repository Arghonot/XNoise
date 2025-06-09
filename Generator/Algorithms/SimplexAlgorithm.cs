using LibNoise;
using LibNoise.Generator;
using Unity.Mathematics;
using UnityEngine;

namespace XNoise
{
    public static class NoiseUtils
    {
        public static Vector3 XXX(Vector3 vector) => new Vector3(vector.x, vector.x, vector.x);
        public static Vector3 YYY(Vector3 vector) => new Vector3(vector.y, vector.y, vector.y);
        public static Vector3 ZZZ(Vector3 vector) => new Vector3(vector.z, vector.z, vector.z);
        public static Vector4 YYYY(float y) => Vector4.one * y;
        public static float Step(float a, float b) => a >= b ? 1f : 0f;
        public static Vector3 Step(Vector3 edge, Vector3 x) => new(x.x >= edge.x ? 1f : 0f, x.y >= edge.y ? 1f : 0f, x.z >= edge.z ? 1f : 0f);
        public static Vector4 Step(Vector4 edge, Vector4 x) => new(x.x >= edge.x ? 1f : 0f, x.y >= edge.y ? 1f : 0f, x.z >= edge.z ? 1f : 0f, x.w >= edge.w ? 1f : 0f);
        public static Vector4 Step(Vector4 edge, float x) => new(edge.x >= x ? 1f : 0f, edge.y >= x ? 1f : 0f, edge.z >= x ? 1f : 0f, edge.w >= x ? 1f : 0f); // TODO may be broken actually, please compare with HLSL
        public static Vector3 Floor(Vector3 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
        public static Vector4 Floor(Vector4 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z), Mathf.Floor(v.w));
        public static float Dot(Vector3 a, Vector3 b) => Vector3.Dot(a, b);
        public static float Dot(Vector4 a, Vector4 b) => Vector4.Dot(a, b);
        public static Vector4 Max(Vector4 a, float b) => new(Mathf.Max(a.x, b), Mathf.Max(a.y, b), Mathf.Max(a.z, b), Mathf.Max(a.w, b));
        public static Vector4 Abs(Vector4 v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        public static Vector4 TaylorInvSqrt(Vector4 r) =>
            new(1.79284291400159f - 0.85373472095314f * Mathf.Sqrt(r.x),
                1.79284291400159f - 0.85373472095314f * Mathf.Sqrt(r.y),
                1.79284291400159f - 0.85373472095314f * Mathf.Sqrt(r.z),
                1.79284291400159f - 0.85373472095314f * Mathf.Sqrt(r.w));
        public static Vector3 Mod289(Vector3 x) => x - Floor(x / 289f) * 289f;
        public static Vector4 Mod289(Vector4 x) => x - Floor(x / 289f) * 289f;
        public static Vector4 Permute(Vector4 x) => Mod289(Multiply(Add(x * 34f, Vector4.one), x));
        public static Vector4 Add(float a, Vector4 b) => new Vector4(a + b.x, a + b.y, a + b.z, a + b.w);
        public static Vector4 Add(Vector4 a, Vector4 b) => new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Vector4 Add(Vector4 a, float b) => new Vector4(a.x + b, a.y + b, a.z + b, a.w + b);
        public static Vector3 Subtract(float a, Vector4 b) => new Vector4(a - b.x, a - b.y, a - b.z);
        public static Vector3 Subtract(Vector3 a, Vector4 b) => new Vector4(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector4 Multiply(Vector4 a, Vector4 b) => new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        public static Vector4 Multiply(Vector4 a, float b) => new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
        public static Vector4 Min(Vector4 a, Vector4 b) => new Vector4(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z), Mathf.Min(a.w, b.w));
        public static Vector4 Max(Vector4 a, Vector4 b) => new Vector4(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z), Mathf.Max(a.w, b.w));
    }

    public class SimplexAlgorithm : INoiseAlgorithm
    {
        Perlin _perlin;

        public void Configure(SerializableModuleBase owner)
        {
            _perlin = (Perlin)owner;
        }

        public float GetValue(float x, float y, float z)
        {
            return GetPerlin(new Vector3(x, y, z), _perlin.Seed, (float)_perlin.Frequency, (float)_perlin.Lacunarity, (float)_perlin.Persistence, _perlin.OctaveCount);
            //return snoise(new Vector3(x, y, z));
        }

        public static float snoise(Vector3 v)
        {
            Vector2 C = new Vector2(0.166666666666666667f, 0.333333333333333333f);
            Vector4 D = new Vector4(0.0f, 0.5f, 1.0f, 2.0f);

            // First corner
            Vector3 i = NoiseUtils.Floor(NoiseUtils.Add(v, NoiseUtils.Dot(v, NoiseUtils.YYY(C))));
            Vector3 x0 = NoiseUtils.Subtract(v, NoiseUtils.Add(i, NoiseUtils.Dot(i, NoiseUtils.XXX(C))));

            // Other corners
            Vector3 g = NoiseUtils.Step(new Vector3(x0.y, x0.z, x0.x), x0);
            Vector3 l = NoiseUtils.Subtract(1f, g);
            Vector3 i1 = NoiseUtils.Min(g, new Vector3(l.z, l.x, l.y));
            Vector3 i2 = NoiseUtils.Max(g, new Vector3(l.z, l.x, l.y));
            Vector3 x1 = x0 - i1 + NoiseUtils.XXX(C);
            Vector3 x2 = x0 - i2 + NoiseUtils.YYY(C);
            Vector3 x3 = x0 - NoiseUtils.YYY(D);

            // Permutations
            i = NoiseUtils.Mod289(i);
            Vector4 p = NoiseUtils.Permute(
                NoiseUtils.Permute(
                    NoiseUtils.Add(NoiseUtils.Add(NoiseUtils.Add(NoiseUtils.Permute(
                            NoiseUtils.Add(i.z, new Vector4(0.0f, i1.z, i2.z, 1.0f))
                    ), i.y), new Vector4(0.0f, i1.y, i2.y, 1.0f)
                ), i.x) + new Vector4(0.0f, i1.x, i2.x, 1.0f)));

            // Gradients: 7x7 points over a square, mapped onto an octahedron.
            // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
            float n_ = 0.142857142857f; // 1/7
            Vector3 ns = n_ * new Vector3(D.w, D.y, D.z) - new Vector3(D.x, D.z, D.x);

            Vector4 j = NoiseUtils.Subtract(p, 49.0f * NoiseUtils.Floor(NoiseUtils.Multiply(NoiseUtils.Multiply(p, ns.z), ns.z)));

            Vector4 x_ = NoiseUtils.Floor(j * ns.z);
            Vector4 y_ = NoiseUtils.Floor(j - 7.0f * x_);

            Vector4 x = x_ * ns.x + NoiseUtils.YYYY(ns.y);
            Vector4 y = y_ * ns.x + NoiseUtils.YYYY(ns.y);
            Vector4 h = NoiseUtils.Subtract(1.0f, NoiseUtils.Subtract(NoiseUtils.Abs(x), NoiseUtils.Abs(y)));

            Vector4 b0 = new Vector4(x.x, x.y, y.x, y.y);
            Vector4 b1 = new Vector4(x.z, x.w, y.z, y.w);

            Vector4 s0 = NoiseUtils.Add(NoiseUtils.Multiply(NoiseUtils.Floor(b0), 2.0f), 1.0f);
            Vector4 s1 = NoiseUtils.Add(NoiseUtils.Multiply(NoiseUtils.Floor(b1), 2.0f), 1.0f);
            Vector4 sh = -NoiseUtils.Step(h, 0.0f);

            Vector4 a0 = NoiseUtils.Add(new Vector4(b0.x, b0.z, b0.y, b0.w), NoiseUtils.Multiply(new Vector4(s0.x, s0.z, s0.y, s0.w), new Vector4(sh.x, sh.x, sh.y, sh.y)));
            Vector4 a1 = new Vector4(b1.x, b1.z , b1.y , b1.w) + NoiseUtils.Multiply(new Vector4(s1.x, s1.z, s1.y, s1.w), new Vector4(sh.z, sh.z, sh.w, sh.w));

            Vector3 p0 = new Vector3(a0.x, a0.y, h.x);
            Vector3 p1 = new Vector3(a0.z, a0.w, h.y);
            Vector3 p2 = new Vector3(a1.x, a1.y, h.z);
            Vector3 p3 = new Vector3(a1.z, a1.w, h.w);

            //Normalise gradients
            Vector4 norm = NoiseUtils.TaylorInvSqrt(new Vector4(
                 NoiseUtils.Dot(p0, p0),
                NoiseUtils.Dot(p1, p1),
                NoiseUtils.Dot(p2, p2),
                NoiseUtils.Dot(p3, p3)
            ));
            p0 *= norm.x;
            p1 *= norm.y;
            p2 *= norm.z;
            p3 *= norm.w;

            // Mix final noise value
            Vector4 m = NoiseUtils.Max(
                NoiseUtils.Subtract(0.6f, new Vector4(
                    NoiseUtils.Dot(x0, x0),
                    NoiseUtils.Dot(x1, x1),
                    NoiseUtils.Dot(x2, x2),
                    NoiseUtils.Dot(x3, x3)
                )),
                0.0f
            );
            m = NoiseUtils.Multiply(m, m);
            return 42.0f * NoiseUtils.Dot(
                NoiseUtils.Multiply(m, m),
                new Vector4(
                    NoiseUtils.Dot(p0, x0),
                    NoiseUtils.Dot(p1, x1),
                    NoiseUtils.Dot(p2, x2),
                    NoiseUtils.Dot(p3, x3)
                )
            );
        }

        float GetPerlin(Vector3 position, float seed, float frequency, float lacunarity, float persistence, float octaves)
        {
            float value = 0.0f;
            float signal = 0.0f;
            float curPersistence = 1.0f;
            float nx, ny, nz;

            position.x *= frequency;
            position.y *= frequency;
            position.z *= frequency;

            for (int curOctave = 0; curOctave < octaves; curOctave++)
            {
                nx = position.x;
                ny = position.y;
                nz = position.z;

                signal = snoise(new Vector3(nx + seed, ny + seed, nz + seed));
                value += signal * curPersistence;

                // Prepare the next octave.
                position.x *= lacunarity;
                position.y *= lacunarity;
                position.z *= lacunarity;
                curPersistence *= persistence;
            }

            return value;
        }
    }
}