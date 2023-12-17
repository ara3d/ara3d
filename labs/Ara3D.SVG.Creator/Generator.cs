using Ara3D.Math;
using Svg;

namespace Ara3D.SVG.Creator;

public abstract class Generator
{
    public Vector2 Center { get; set; }
    public Vector2 Size { get; set; }
    public abstract SvgElement ToSvg();
}

public class Rectangle : Generator
{
    public override SvgElement ToSvg()
    {
        return Size.ToRectangle().Offset(Center);
    }
}

public class Ellipse : Generator
{
    public override SvgElement ToSvg()
    {
        return Size.ToEllipse().Offset(Center);
    }
}

