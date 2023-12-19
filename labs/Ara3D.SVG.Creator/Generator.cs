using Ara3D.Math;
using ExCSS;
using Svg;
using Svg.Pathing;

namespace Ara3D.SVG.Creator;

public abstract class Generator
{
    public Vector2 Center
    {
        get => A + Size / 2;
        set
        {
            var offset = Center - value;
            A += offset;
            B += offset;
        }
    }

    public Vector2 Size
    {
        get => B - A;
        set
        {
            A = Center - value / 2;
            B = Center + value / 2;
        }
    }

    public Vector2 A { get; set; }
    public Vector2 B { get; set; }

    public abstract IEntity Evaluate();
}

public enum SvgPrimitiveClosedShapeEnum
{
    Ellipse,
    Rect, 
    Square,
    Circle,
}

public class EllipseGenerator : SvgPrimitiveClosedShape
{
    public EllipseGenerator()
        => Shape = SvgPrimitiveClosedShapeEnum.Ellipse;
}

public class CircleGenerator : SvgPrimitiveClosedShape
{
    public CircleGenerator()
        => Shape = SvgPrimitiveClosedShapeEnum.Circle;
}

public class RectGenerator : SvgPrimitiveClosedShape
{
    public RectGenerator()
        => Shape = SvgPrimitiveClosedShapeEnum.Rect;
}

public class SquareGenerator : SvgPrimitiveClosedShape
{
    public SquareGenerator()
        => Shape = SvgPrimitiveClosedShapeEnum.Square;
}

public class SvgPrimitiveClosedShape : Generator
{
    public SvgPrimitiveClosedShapeEnum Shape { get; set; } = SvgPrimitiveClosedShapeEnum.Rect;

    public SvgEntity CreateSvgEntity()
    {
        if (Shape == SvgPrimitiveClosedShapeEnum.Ellipse)
        {
            return new SvgEllipse()
            {
                CenterX = Center.X,
                CenterY = Center.Y,
                RadiusX = Size.X / 2,
                RadiusY = Size.Y / 2
            };
        }
        if (Shape == SvgPrimitiveClosedShapeEnum.Circle)
        {
            return new SvgEllipse()
            {
                CenterX = Center.X,
                CenterY = Center.Y,
                RadiusX = Size.MinComponent() / 2,
                RadiusY = Size.MinComponent() / 2
            };
        }
        else if (Shape == SvgPrimitiveClosedShapeEnum.Square)
        {
            return new SvgRectangle()
            {
                X = A.X,
                Y = A.Y,
                Width = Size.MinComponent(),
                Height = Size.MinComponent()
            };
        }
        else
        {
            return new SvgRectangle()
            {
                X = A.X,
                Y = A.Y,
                Width = Size.X,
                Height = Size.Y
            };
        }
    }

    public override IEntity Evaluate() => CreateSvgEntity();
}

public class FunctionGenerator : Generator
{
    public FunctionRendererParameters RendererParameters { get; set; } = new();
    public Function Function { get; set; } = new Circle();
    public Vector2 GetPoint(float x) => A + Function.Func(x * Function.Length + Function.Offset) * Size;

    public override IEntity Evaluate()
    {
        var group = new SvgGroup();
        var n = RendererParameters.NumSamples;

        if (RendererParameters.AsPointsOrLines)
        {
            group.Children.Clear();

            for (var i = 0f; i <= n; ++i)
            {
                var v = GetPoint(i / n);

                var circle = new SvgCircle();
                circle.CenterX = v.X;
                circle.CenterY = v.Y;
                circle.Radius = (float)RendererParameters.OuterThickness;
                circle.Fill = new SvgColourServer(RendererParameters.StrokeColor);
                group.Children.Add(circle);
            }

            for (var i = 0f; i <= n; ++i)
            {
                var v = GetPoint(i / n);

                var circle = new SvgCircle();
                circle.CenterX = v.X;
                circle.CenterY = v.Y;
                circle.Radius = (float)RendererParameters.InnerThickness;
                circle.Fill = new SvgColourServer(RendererParameters.FillColor);
                group.Children.Add(circle);
            }
        }
        else
        {
            var path1 = new SvgPath();
            var path2 = new SvgPath();

            path1.PathData = new SvgPathSegmentList();

            var v = GetPoint(0);
            path1.PathData.Add(new SvgMoveToSegment(false, v.ToSvg()));
            for (var i = 1f; i <= n; i += 1)
            {
                v = GetPoint(i / n);
                path1.PathData.Add(new SvgLineSegment(false, v.ToSvg()));
            }

            path1.StrokeWidth = (float)RendererParameters.OuterThickness;
            path1.Fill = SvgPaintServer.None;
            path1.Stroke = new SvgColourServer(RendererParameters.FillColor);

            path2.PathData = path1.PathData;
            path2.StrokeWidth = (float)RendererParameters.InnerThickness;
            path2.Fill = SvgPaintServer.None;
            path2.Stroke = new SvgColourServer(RendererParameters.StrokeColor);

            group.Children.Clear();
            group.Children.Add(path1);
            group.Children.Add(path2);
        }

        return (SvgEntity)group;
    }
}

