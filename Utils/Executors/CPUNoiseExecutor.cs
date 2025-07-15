using LibNoise;
using System;
using UnityEngine;

namespace XNoise
{
    public class CPUNoiseExecutor : NoiseExecutor
    {
        #region Fields

        private float[,] _data;
        private readonly float[,] _ucData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of CPUNoiseExecutor.
        /// </summary>
        protected CPUNoiseExecutor() : base() { }

        /// <summary>
        /// Initializes a new instance of CPUNoiseExecutor.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        public CPUNoiseExecutor(int size) : base(size, size, null) { }

        /// <summary>
        /// Initializes a new instance of CPUNoiseExecutor.
        /// </summary>
        /// <param name="size">The width and height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public CPUNoiseExecutor(int size, INoiseStrategy generator) : base(size, size, generator) { }

        /// <summary>
        /// Initializes a new instance of CPUNoiseExecutor.
        /// </summary>
        /// <param name="width">The width of the noise map.</param>
        /// <param name="height">The height of the noise map.</param>
        /// <param name="generator">The generator module.</param>
        public CPUNoiseExecutor(int width, int height, INoiseStrategy generator = null) : base(width, height)
        {
            _generator = generator;
            _data = new float[width, height];
            _ucData = new float[width + _ucBorder * 2, height + _ucBorder * 2];
        }


        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets a value in the noise map by its position.
        /// </summary>
        /// <param name="x">The position on the x-axis.</param>
        /// <param name="y">The position on the y-axis.</param>
        /// <param name="isCropped">Indicates whether to select the cropped (default) or uncropped noise map data.</param>
        /// <returns>The corresponding value.</returns>
        public float this[int x, int y, bool isCropped = true]
        {
            get
            {
                if (isCropped)
                {
                    if (x < 0 && x >= _width)
                    {
                        throw new ArgumentOutOfRangeException("Invalid x position");
                    }
                    if (y < 0 && y >= _height)
                    {
                        throw new ArgumentOutOfRangeException("Invalid y position");
                    }
                    return _data[x, y];
                }
                if (x < 0 && x >= _ucWidth)
                {
                    throw new ArgumentOutOfRangeException("Invalid x position");
                }
                if (y < 0 && y >= _ucHeight)
                {
                    throw new ArgumentOutOfRangeException("Invalid y position");
                }
                return _ucData[x, y];
            }
            set
            {
                if (isCropped)
                {
                    if (x < 0 && x >= _width)
                    {
                        throw new ArgumentOutOfRangeException("Invalid x position");
                    }
                    if (y < 0 && y >= _height)
                    {
                        throw new ArgumentOutOfRangeException("Invalid y position");
                    }
                    _data[x, y] = value;
                }
                else
                {
                    if (x < 0 && x >= _ucWidth)
                    {
                        throw new ArgumentOutOfRangeException("Invalid x position");
                    }
                    if (y < 0 && y >= _ucHeight)
                    {
                        throw new ArgumentOutOfRangeException("Invalid y position");
                    }
                    _ucData[x, y] = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets normalized noise map data with all values in the set of {0..1}.
        /// </summary>
        /// <param name="isCropped">Indicates whether to select the cropped (default) or uncropped noise map data.</param>
        /// <param name="xCrop">This value crops off data from the right of the noise map data.</param>
        /// <param name="yCrop">This value crops off data from the bottom of the noise map data.</param>
        /// <returns>The normalized noise map data.</returns>
        public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
        {
            return GetData(isCropped, xCrop, yCrop, true);
        }

        /// <summary>
        /// Gets noise map data.
        /// </summary>
        /// <param name="isCropped">Indicates whether to select the cropped (default) or uncropped noise map data.</param>
        /// <param name="xCrop">This value crops off data from the right of the noise map data.</param>
        /// <param name="yCrop">This value crops off data from the bottom of the noise map data.</param>
        /// <param name="isNormalized">Indicates whether to normalize noise map data.</param>
        /// <returns>The noise map data.</returns>
        public float[,] GetData(bool isCropped = true, int xCrop = 0, int yCrop = 0, bool isNormalized = false)
        {
            int width, height;
            float[,] data;
            if (isCropped)
            {
                width = _width;
                height = _height;
                data = _data;
            }
            else
            {
                width = _ucWidth;
                height = _ucHeight;
                data = _ucData;
            }
            width -= xCrop;
            height -= yCrop;
            var result = new float[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    float sample;
                    if (isNormalized)
                    {
                        sample = (data[x, y] + 1) / 2;
                    }
                    else
                    {
                        sample = data[x, y];
                    }
                    result[x, y] = sample;
                }
            }
            return result;
        }

        /// <summary>
        /// Clears the noise map.
        /// </summary>
        /// <param name="value">The constant value to clear the noise map with.</param>
        public void Clear(float value = 0f)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _data[x, y] = value;
                }
            }
        }

        /// <summary>
        /// Generates a planar projection of a point in the noise map.
        /// </summary>
        /// <param name="x">The position on the x-axis.</param>
        /// <param name="y">The position on the y-axis.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GeneratePlanar(double x, double y)
        {
            return _generator.GetValueCPU(x, 0.0, y);
        }

        /// <summary>
        /// Generates a non-seamless planar projection of the noise map.
        /// </summary>
        /// <param name="left">The clip region to the left.</param>
        /// <param name="right">The clip region to the right.</param>
        /// <param name="top">The clip region to the top.</param>
        /// <param name="bottom">The clip region to the bottom.</param>
        /// <param name="isSeamless">Indicates whether the resulting noise map should be seamless.</param>
        public override void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless = true)
        {
            base.GeneratePlanar(left, right, top, bottom, isSeamless);
            var xe = right - left;
            var ze = bottom - top;
            var xd = xe / ((double)_width - _ucBorder);
            var zd = ze / ((double)_height - _ucBorder);
            var xc = left;
            for (var x = 0; x < _ucWidth; x++)
            {
                var zc = top;
                for (var y = 0; y < _ucHeight; y++)
                {
                    float fv;
                    if (!isSeamless)
                    {
                        fv = (float)GeneratePlanar(xc, zc);
                    }
                    else
                    {
                        var swv = GeneratePlanar(xc, zc);
                        var sev = GeneratePlanar(xc + xe, zc);
                        var nwv = GeneratePlanar(xc, zc + ze);
                        var nev = GeneratePlanar(xc + xe, zc + ze);
                        var xb = 1.0 - ((xc - left) / xe);
                        var zb = 1.0 - ((zc - top) / ze);
                        var z0 = Utils.InterpolateLinear(swv, sev, xb);
                        var z1 = Utils.InterpolateLinear(nwv, nev, xb);
                        fv = (float)Utils.InterpolateLinear(z0, z1, zb);
                    }
                    _ucData[x, y] = fv;
                    if (x >= _ucBorder && y >= _ucBorder && x < _width + _ucBorder &&
                        y < _height + _ucBorder)
                    {
                        _data[x - _ucBorder, y - _ucBorder] = fv; // Cropped data
                    }
                    zc += zd;
                }
                xc += xd;
            }
        }

        /// <summary>
        /// Generates a cylindrical projection of a point in the noise map.
        /// </summary>
        /// <param name="angle">The angle of the point.</param>
        /// <param name="height">The height of the point.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GenerateCylindrical(double angle, double height)
        {
            var x = Math.Cos(angle * Mathf.Deg2Rad);
            var y = height;
            var z = Math.Sin(angle * Mathf.Deg2Rad);
            return _generator.GetValueCPU(x, y, z);
        }

        /// <summary>
        /// Generates a cylindrical projection of the noise map.
        /// </summary>
        /// <param name="angleMin">The maximum angle of the clip region.</param>
        /// <param name="angleMax">The minimum angle of the clip region.</param>
        /// <param name="heightMin">The minimum height of the clip region.</param>
        /// <param name="heightMax">The maximum height of the clip region.</param>
        public override void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
        {
            if (angleMax <= angleMin || heightMax <= heightMin)
            {
                throw new ArgumentException("Invalid angle or height parameters");
            }
            if (_generator == null)
            {
                throw new ArgumentNullException("Generator is null");
            }
            var ae = angleMax - angleMin;
            var he = heightMax - heightMin;
            var xd = ae / ((double)_width - _ucBorder);
            var yd = he / ((double)_height - _ucBorder);
            var ca = angleMin;
            for (var x = 0; x < _ucWidth; x++)
            {
                var ch = heightMin;
                for (var y = 0; y < _ucHeight; y++)
                {
                    _ucData[x, y] = (float)GenerateCylindrical(ca, ch);
                    if (x >= _ucBorder && y >= _ucBorder && x < _width + _ucBorder &&
                        y < _height + _ucBorder)
                    {
                        _data[x - _ucBorder, y - _ucBorder] = (float)GenerateCylindrical(ca, ch);
                        // Cropped data
                    }
                    ch += yd;
                }
                ca += xd;
            }
        }

        /// <summary>
        /// Generates a spherical projection of a point in the noise map.
        /// </summary>
        /// <param name="lat">The latitude of the point.</param>
        /// <param name="lon">The longitude of the point.</param>
        /// <returns>The corresponding noise map value.</returns>
        private double GenerateSpherical(double lat, double lon)
        {
            var r = Math.Cos(Mathf.Deg2Rad * lat);
            return _generator.GetValueCPU(r * Math.Cos(Mathf.Deg2Rad * lon), Math.Sin(Mathf.Deg2Rad * lat),
                r * Math.Sin(Mathf.Deg2Rad * lon));
        }

        /// <summary>
        /// Generates a spherical projection of the noise map.
        /// </summary>
        /// <param name="south">The clip region to the south.</param>
        /// <param name="north">The clip region to the north.</param>
        /// <param name="west">The clip region to the west.</param>
        /// <param name="east">The clip region to the east.</param>
        public override void GenerateSpherical(double south, double north, double west, double east)
        {
            if (east <= west || south <= north)
            {
                throw new ArgumentException("Invalid east/west or north/south combination");
            }
            if (_generator == null)
            {
                throw new ArgumentNullException("Generator is null");
            }
            var loe = east - west;
            var lae = north - south;
            var xd = loe / ((double)_width - _ucBorder);
            var yd = lae / ((double)_height - _ucBorder);
            var clo = west;
            for (var x = 0; x < _ucWidth; x++)
            {
                var cla = south;
                for (var y = 0; y < _ucHeight; y++)
                {
                    _ucData[x, y] = (float)GenerateSpherical(cla, clo);
                    if (x >= _ucBorder && y >= _ucBorder && x < _width + _ucBorder &&
                        y < _height + _ucBorder)
                    {
                        _data[x - _ucBorder, y - _ucBorder] = (float)GenerateSpherical(cla, clo);
                        // Cropped data
                    }
                    cla += yd;
                }
                clo += xd;
            }
        }

        /// <summary>
        /// Creates a single channel texture map for the current content of the noise map.
        /// </summary>
        /// <returns>The created texture map.</returns>
        public override Texture2D GetTexture()
        {
            var texture = new Texture2D(_width, _height, TextureFormat.R8, false);
            var pixels = new Color[_width * _height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    float sample;
                    if (!float.IsNaN(_borderValue) &&
                        (x == 0 || x == _width - _ucBorder || y == 0 || y == _height - _ucBorder))
                    {
                        sample = _borderValue;
                    }
                    else
                    {
                        sample = _data[x, y];
                    }
                    pixels[x + y * _width] = new Color((sample + 1) / 2, 0f, 0f, 0f);
                }
            }
            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }


        /// <summary>
        /// Creates a texture map for the current content of the noise map.
        /// </summary>
        /// <param name="gradient">The gradient to color the texture map with.</param>
        /// <returns>The created texture map.</returns>
        public override Texture2D GetFinalizedTexture(object gradient)
        {
            
            var texture = new Texture2D(_width, _height);
            var pixels = new Color[_width * _height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    float sample;
                    if (!float.IsNaN(_borderValue) &&
                        (x == 0 || x == _width - _ucBorder || y == 0 || y == _height - _ucBorder))
                    {
                        sample = _borderValue;
                    }
                    else
                    {
                        sample = _data[x, y];
                    }
                    pixels[x + y * _width] = ((Gradient)gradient).Evaluate((sample + 1) / 2);
                }
            }
            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Creates a normal map for the current content of the noise map.
        /// </summary>
        /// <param name="intensity">The scaling of the normal map values.</param>
        /// <returns>The created normal map.</returns>
        public override Texture2D GetNormalMap(float intensity)
        {
            var texture = new Texture2D(_width, _height);
            var pixels = new Color[_width * _height];
            for (var x = 0; x < _ucWidth; x++)
            {
                for (var y = 0; y < _ucHeight; y++)
                {
                    var xPos = (_ucData[Mathf.Max(0, x - _ucBorder), y] -
                                _ucData[Mathf.Min(x + _ucBorder, _width + _ucBorder), y]) / 2;
                    var yPos = (_ucData[x, Mathf.Max(0, y - _ucBorder)] -
                                _ucData[x, Mathf.Min(y + _ucBorder, _height + _ucBorder)]) / 2;
                    var normalX = new Vector3(xPos * intensity, 0, 1);
                    var normalY = new Vector3(0, yPos * intensity, 1);
                    // Get normal vector
                    var normalVector = normalX + normalY;
                    normalVector.Normalize();
                    // Get color vector
                    var colorVector = Vector3.zero;
                    colorVector.x = (normalVector.x + 1) / 2;
                    colorVector.y = (normalVector.y + 1) / 2;
                    colorVector.z = (normalVector.z + 1) / 2;
                    // Start at (x + _ucBorder, y + _ucBorder) so that resulting normal map aligns with cropped data
                    if (x >= _ucBorder && y >= _ucBorder && x < _width + _ucBorder &&
                        y < _height + _ucBorder)
                    {
                        pixels[(x - _ucBorder) + (y - _ucBorder) * _width] = new Color(colorVector.x,
                            colorVector.y, colorVector.z);
                    }
                }
            }
            texture.SetPixels(pixels);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            return texture;
        }

        #endregion

        #region IDisposable Members

        protected override bool Disposing()
        {
            if (!base.Disposing()) return false;
            _data = null;
            return true;
        }

        #endregion
    }
}