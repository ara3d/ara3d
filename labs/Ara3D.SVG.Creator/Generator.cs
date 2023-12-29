using Ara3D.Math;
using Svg;
using Svg.Pathing;

namespace Ara3D.SVG.Creator;

public abstract class Generator
{
    public Position Center
    {
        get => A + Size.ToVector() / 2;
        set
        {
            var offset = value - Center.ToVector();
            A += offset;
            B += offset;
        }
    }

    public Size Size
    {
        get => B.ToVector() - A;
        set
        {
            var center = Center.ToVector();
            A = center - value.ToVector() / 2;
            B = center + value.ToVector() / 2;
        }
    }

    public Position A { get; set; } = DVector2.Zero;
    public Position B { get; set; } = DVector2.Zero;

    public abstract IEntity Evaluate();
}

public enum SvgPrimitiveClosedShapeEnum
{
    Ellipse,
    Rect, 
    Square,
    Circle,
}

public class StarShape : Generator
{
    public int Points { get; set; } = 5;

    public double InnerRadius { get; set; } = 20;
    public double OuterRadius { get; set; } = 50;

    public override IEntity Evaluate()
    {
        var points = new List<DVector2>();
        var n = Points * 2;
        for (var i = 0; i < n; i++)
        {
            var theta = System.Math.PI * 2 * i / (double)n;
            var r = i.IsEven() ? OuterRadius : InnerRadius;
            var pt = new DVector2(theta.Sin(), theta.Cos()) * r;
            pt += Center;
            points.Add(pt);
        }

        return (SvgEntity)points.ToSvgPath(true);
    }
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
                CenterX = (float)Center.X,
                CenterY = (float)Center.Y,
                RadiusX = (float)Size.W / 2,
                RadiusY = (float)Size.H / 2
            };
        }
        if (Shape == SvgPrimitiveClosedShapeEnum.Circle)
        {
            return new SvgEllipse()
            {
                CenterX = (float)Center.X,
                CenterY = (float)Center.Y,
                RadiusX = (float)Size.ToVector().MinComponent() / 2,
                RadiusY = (float)Size.ToVector().MinComponent() / 2
            };
        }
        else if (Shape == SvgPrimitiveClosedShapeEnum.Square)
        {
            return new SvgRectangle()
            {
                X = (float)A.X,
                Y = (float)A.Y,
                Width = (float)Size.ToVector().MinComponent(),
                Height = (float)Size.ToVector().MinComponent()
            };
        }
        else
        {
            return new SvgRectangle()
            {
                X = (float)A.X,
                Y = (float)A.Y,
                Width = (float)Size.W,
                Height = (float)Size.H
            };
        }
    }

    public override IEntity Evaluate() => CreateSvgEntity();
}


/*
public class RawSvg : Generator
{
    public string Svg { get; set; }
    //= @"<svg fill=""#E4202E"" role=""img"" viewBox=""0 0 24 24"" xmlns=""http://www.w3.org/2000/svg""><title>Atari</title><path d=""M0 21.653s3.154-.355 5.612-2.384c2.339-1.93 3.185-3.592 3.77-5.476.584-1.885.671-6.419.671-7.764V2.346H8.598v1.365c-.024 2.041-.2 5.918-1.135 8.444C5.203 18.242 0 18.775 0 18.775zm24 0s-3.154-.355-5.61-2.384c-2.342-1.93-3.187-3.592-3.772-5.476-.583-1.885-.671-6.419-.671-7.764V2.346H15.4l.001 1.365c.024 2.041.202 5.918 1.138 8.444 2.258 6.087 7.46 6.62 7.46 6.62zM10.659 2.348h2.685v19.306H10.66Z""/></svg>";
        = File.ReadAllText(@"C:/Users/cdigg/git/temp/noto-emoji/svg/emoji_u2708.svg");
    public override IEntity Evaluate()
        => SvgEntity.Create(Svg);
}*/

public class RawSvg : Generator
{
    public string FilePath { get; set; } = @"C:/Users/cdigg/git/temp/noto-emoji/svg/emoji_u2708.svg";

    public override IEntity Evaluate()
        => SvgEntity.LoadFromFile(FilePath);
}

public class FunctionGenerator : Generator
{
    public FunctionRendererParameters RendererParameters { get; set; } = new();
    public Function Function { get; set; } = new CircleFunc();
    public DVector2 GetPoint(float x) => A + Function.Func(x * Function.Length + Function.Offset) * Size;

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
                circle.CenterX = (float)v.X;
                circle.CenterY = (float)v.Y;
                circle.Radius = (float)RendererParameters.OuterThickness;
                circle.Fill = new SvgColourServer(RendererParameters.StrokeColor);
                group.Children.Add(circle);
            }

            for (var i = 0f; i <= n; ++i)
            {
                var v = GetPoint(i / n);

                var circle = new SvgCircle();
                circle.CenterX = (float)v.X;
                circle.CenterY = (float)v.Y;
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

public static class SvgExtensions
{
    public static SvgPath ToSvgPath(this IReadOnlyList<DVector2> points, bool closed)
    {
        var r = new SvgPath();
        if (points.Count > 0)
        {
            r.PathData = new SvgPathSegmentList { new SvgMoveToSegment(false, points[0].ToSvg()) };
            for (var i = 1; i < points.Count; i += 1)
            {
                r.PathData.Add(new SvgLineSegment(false, points[i].ToSvg()));
            }

            if (closed)
            {
                r.PathData.Add(new SvgLineSegment(false, points[0].ToSvg()));
            }
        }
        return r;
    }
}