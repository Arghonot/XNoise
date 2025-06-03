
using LibNoise;

namespace Xnoise
{
    // base class for any 3d noise sampling algorithm
    public interface INoiseAlgorithm
    {
        float GetValue(float x, float y, float z);
        void Configure(SerializableModuleBase owner);
    }
}