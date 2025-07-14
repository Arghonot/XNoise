using LibNoise;
using System;
using System.Xml.Serialization;
using UnityEngine;

namespace XNoise
{
    public abstract class NoiseExecutor : IDisposable
    {
        #region Constants

        public static readonly double South = -90.0;
        public static readonly double North = 90.0;
        public static readonly double West = -180.0;
        public static readonly double East = 180.0;
        public static readonly double AngleMin = -180.0;
        public static readonly double AngleMax = 180.0;
        public static readonly double Left = -1.0;
        public static readonly double Right = 1.0;
        public static readonly double Top = -1.0;
        public static readonly double Bottom = 1.0;

        #endregion

        #region Fields

        protected int _width;
        protected int _height;

        protected readonly int _ucWidth;
        protected readonly int _ucHeight;
        protected int _ucBorder = 1; // Border size of extra noise for uncropped data.

        protected float _borderValue = float.NaN;
        protected ModuleBase _generator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        protected NoiseExecutor() { }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        public NoiseExecutor(int size) : this(size, size, null) { }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public NoiseExecutor(int size, ModuleBase generator) : this(size, size, generator) { }

        /// <summary>
        /// Initializes a new instance of Noise2D.
        /// </summary>
        /// <param name="width">The width of the noise map.</param>
        /// <param name="height">The height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public NoiseExecutor(int width, int height, ModuleBase generator = null)
        {
            _generator = generator;
            _width = width;
            _height = height;
            _ucWidth = width + _ucBorder * 2;
            _ucHeight = height + _ucBorder * 2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the constant value at the noise maps borders.
        /// </summary>
        public float Border
        {
            get { return _borderValue; }
            set { _borderValue = value; }
        }

        /// <summary>
        /// Gets or sets the generator module.
        /// </summary>
        public ModuleBase Generator
        {
            get { return _generator; }
            set { _generator = value; }
        }

        /// <summary>
        /// Gets the height of the noise map.
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Gets the width of the noise map.
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a non-seamless planar projection of the noise map.
        /// </summary>
        /// <param name="left">The clip region to the left.</param>
        /// <param name="right">The clip region to the right.</param>
        /// <param name="top">The clip region to the top.</param>
        /// <param name="bottom">The clip region to the bottom.</param>
        /// <param name="isSeamless">Indicates whether the resulting noise map should be seamless.</param>
        public abstract void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true);

        /// <summary>
        /// Generates a cylindrical projection of the noise map.
        /// </summary>
        /// <param name="angleMin">The maximum angle of the clip region.</param>
        /// <param name="angleMax">The minimum angle of the clip region.</param>
        /// <param name="heightMin">The minimum height of the clip region.</param>
        /// <param name="heightMax">The maximum height of the clip region.</param>
        public abstract void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax);

        /// <summary>
        /// Generates a spherical projection of the noise map.
        /// </summary>
        /// <param name="south">The clip region to the south.</param>
        /// <param name="north">The clip region to the north.</param>
        /// <param name="west">The clip region to the west.</param>
        /// <param name="east">The clip region to the east.</param>
        public abstract void GenerateSpherical(double south, double north, double west, double east);

        /// <summary>
        /// Creates a grayscale texture map for the current content of the noise map.
        /// </summary>
        /// <returns>The created texture map.</returns>
        public abstract Texture2D GetTexture();

        /// <summary>
        /// Creates a texture map for the current content of the noise map.
        /// </summary>
        /// <param name="gradient">The gradient to color the texture map with.</param>
        /// <returns>The created texture map.</returns>
        public abstract Texture2D GetTexture(Gradient gradient);

        /// <summary>
        /// Creates a normal map for the current content of the noise map.
        /// </summary>
        /// <param name="intensity">The scaling of the normal map values.</param>
        /// <returns>The created normal map.</returns>
        public abstract Texture2D GetNormalMap(float intensity);

        #endregion

        #region IDisposable Members

        [XmlIgnore]
#if !XBOX360 && !ZUNE
        [NonSerialized]
#endif
        private bool _disposed;

        /// <summary>
        /// Gets a value whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = Disposing();
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Immediately releases the unmanaged resources used by this object.
        /// </summary>
        /// <returns>True if the object is completely disposed.</returns>
        protected virtual bool Disposing()
        {
            _width = 0;
            _height = 0;
            return true;
        }

        #endregion
    }
}