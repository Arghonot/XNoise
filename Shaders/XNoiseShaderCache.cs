using System.Collections.Generic;
using UnityEngine;

namespace Xnoise
{
    public static class XNoiseShaderCache
    {
        private static readonly Dictionary<string, Shader> _shaders = new();
        private static readonly Dictionary<string, Material> _materials = new();

        /// <summary>
        /// Gets the shader by name. Automatically caches after first lookup.
        /// </summary>
        public static Shader GetShader(string shaderPath)
        {
            if (_shaders.TryGetValue(shaderPath, out var shader))
                return shader;

            shader = Shader.Find(shaderPath);
            if (shader == null)
            {
                Debug.LogError($"Shader not found: {shaderPath}");
                return null;
            }

            _shaders[shaderPath] = shader;
            return shader;
        }

        /// <summary>
        /// Gets a cached material using the shader path. Creates it on first call.
        /// </summary>
        public static Material GetMaterial(string shaderPath)
        {
            if (_materials.TryGetValue(shaderPath, out var material))
                return material;

            var shader = GetShader(shaderPath);
            if (shader == null)
                return null;

            material = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _materials[shaderPath] = material;
            return material;
        }

        /// <summary>
        /// Optional: Clears all cached shaders/materials (e.g. on editor refresh or exit).
        /// </summary>
        public static void Clear()
        {
            foreach (var mat in _materials.Values)
            {
                if (Application.isEditor)
                    Object.DestroyImmediate(mat);
                else
                    Object.Destroy(mat);
            }
            _materials.Clear();
            _shaders.Clear();
        }
    }
}