using Ara3D.Math;
using Svg;

namespace Ara3D.SVG.Creator;

public class Entity
{
    public Generator Generator { get; set; }
    public List<Modifier> Elements { get; set; } = new List<Modifier>();
    public string ToSvg() => throw new NotImplementedException();
}

public static class SvgExtensions
{
    public static SvgEllipse ToEllipse(this Vector2 size)
    {
        return new SvgEllipse()
        {
            RadiusX = size.X,
            RadiusY = size.Y
        };
    }

    public static SvgRectangle ToRectangle(this Vector2 size)
    {
        return new SvgRectangle()
        {
            Width = size.X,
            Height = size.Y
        };
    }

    public static SvgRectangle Offset(this SvgRectangle rect, Vector2 offset)
    {
        rect.X += offset.X;
        rect.Y += offset.Y;
        return rect;
    }

    public static SvgEllipse Offset(this SvgEllipse ellipse, Vector2 offset)
    {
        ellipse.CenterX += offset.X;
        ellipse.CenterY += offset.Y;
        return ellipse;
    }
}