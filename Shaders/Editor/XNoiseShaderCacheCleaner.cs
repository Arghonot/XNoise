using UnityEditor;
using XNoise;

[InitializeOnLoad]
public static class XNoiseShaderCacheCleaner
{
    static XNoiseShaderCacheCleaner()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                XNoiseShaderCache.Clear();
            }
        };
    }
}