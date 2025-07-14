using LibNoise;

public struct RenderingAreaData // Helper for rendering areas/coordinates ?
{
    public double left;
    public double right;
    public double top;
    public double bottom;

    public RenderingAreaData(double a, double b, double c, double d)
    {
        left = a;
        right = b;
        top = c;
        bottom = d;
    }

    public static readonly RenderingAreaData standardSpherical = new RenderingAreaData(Noise2D.West, Noise2D.East, Noise2D.North, Noise2D.South);
    public static readonly RenderingAreaData standardCartesian = new RenderingAreaData(Noise2D.Left, Noise2D.Right, Noise2D.North, Noise2D.South);
    public static readonly RenderingAreaData standardCylindrical = new RenderingAreaData(Noise2D.AngleMin, Noise2D.AngleMax, Noise2D.Top, Noise2D.Bottom);
}