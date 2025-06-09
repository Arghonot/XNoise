namespace XNoise
{
    public static class XNoiseShaderPaths
    {
        // Generators
        public const string Perlin = "Xnoise/Generators/PerlinSurfaceShader";
        public const string Voronoi = "Xnoise/Generators/VoronoiSurfaceShader";
        public const string Billow = "Xnoise/Generators/BillowSurfaceShader";
        public const string RidgedMultifractal = "Xnoise/Generators/RidgedMultifractalSurfaceShader";
        public const string Const = "Xnoise/Generators/ConstSurfaceShader";
        public const string Spheres = "Xnoise/Generators/SpheresSurfaceShader";
        public const string Checker = "Xnoise/Generators/CheckerSurfaceShader";
        public const string Cylinders = "Xnoise/Generators/CylinderSurfaceShader";

        // Modifiers and others
        public const string Binarize = "Xnoise/Modifiers/Binarize";
        public const string ReadImage = "Xnoise/Modifiers/ReadImage";
        public const string Terrace = "Xnoise/Modifiers/Terrace";
        public const string Turbulence = "Xnoise/Transformers/Turbulence";
        public const string Select = "Xnoise/Selectors/Select";
        public const string Subtract = "Xnoise/Combiners/Subtract";
        public const string ScaleBias = "Xnoise/Modifiers/ScaleBias";
        public const string Power = "Xnoise/Combiners/Power";
        public const string Curve = "Xnoise/Modifiers/Curve";
        public const string Multiply = "Xnoise/Combiners/Multiply";
        public const string Min = "Xnoise/Combiners/Min";
        public const string Exponent = "Xnoise/Modifiers/Exponent";
        public const string Displace = "Xnoise/Transformers/Displace";
        public const string Invert = "Xnoise/Modifiers/Invert";
        public const string Max = "Xnoise/Combiners/Max";
        public const string Clamp = "Xnoise/Modifiers/Clamp";
        public const string Blend = "Xnoise/Selectors/Blend";
        public const string Add = "Xnoise/Combiners/Add";
        public const string Abs = "Xnoise/Modifiers/Abs";

        // Misc
        public const string Visualizer = "Xnoise/Visualizer";
    }
}