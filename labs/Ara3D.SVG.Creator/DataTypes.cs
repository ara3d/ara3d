using Ara3D.Math;

namespace Ara3D.SVG.Creator;


public class Size
{
    public Size(double w, double h) => (W, H) = (w, h);
    public double W { get; set; }
    public double H { get; set; }
    public DVector2 ToVector() => (W, H);
    public static implicit operator DVector2(Size s) => new(s.W, s.H);
    public static implicit operator Size(DVector2 v) => new(v.X, v.Y);
}

public class Position
{
    public Position(double x, double y) => (X, Y) = (x, y);
    public double X { get; set; }
    public double Y { get; set; }
    public DVector2 ToVector() => (X, Y);
    public static implicit operator DVector2(Position p) => new(p.X, p.Y);
    public static implicit operator Position(DVector2 v) => new(v.X, v.Y);
}

public class Angle
{
    public double Degrees { get; set; }
    public Angle(double x) => Degrees = x;
    public static implicit operator Angle(double x) => new(x);
    public static implicit operator double(Angle x) => x.Degrees;
}

public class Circle
{
    public Circle(Position center, double r)
        => (Center, R) = (center, r);
    public Position Center;
    public double R;
};

public class Rect
{
    public Rect(Position center, Size size)
        => (Center, Size) = (center, size);
    public Position Center { get; set; }
    public Size Size { get; set; }
}

public class RoundedRect
{
    public RoundedRect(Position center, Size size, Vector radii)
        => (Center, Size, Radii) = (center, size, radii);
    public Position Center { get; set; }
    public Size Size { get; set; }
    public Vector Radii { get; set; }
}

public class Line
{
    public Line(Position a, Position b)
        => (A, B) = (a, b);

    public Position A { get; set; }
    public Position B { get; set; }

    public double L
    {
        get => A.ToVector().Distance(B);
        set
        {
            var center = Center;
            A = A.ToVector().MoveToDistanceFrom(center, value / 2);
            B = B.ToVector().MoveToDistanceFrom(center, value / 2);
        }
    }

    public Position Center
    {
        get => A.ToVector().Average(B.ToVector());
        set
        {
            var delta = value.ToVector() - Center;
            A = A.ToVector() - delta;
            B = B.ToVector() - delta;
        }
    }
}

public class Ellipse
{
    public Ellipse(Position center, Size size)
        => (Center, Size) = (center, size);
    public Position Center { get; set; }
    public Size Size { get; set; }
}

public class Square
{
    public Square(Position center, double l)
        => (Center, L) = (center, l);
    public Position Center { get; set; }
    public double L { get; set; }
};

public class Scale
{
    public Scale(double x, double y)
        => (X, Y) = (x, y);
    public double X { get; set; }
    public double Y { get; set; }
};

public class Vector
{
    public Vector(double x, double y)
        => (X, Y) = (x, y);
    public double X { get; set; }
    public double Y { get; set; }
    public DVector2 ToVector() => (X, Y);
    public static implicit operator DVector2(Vector v) => new(v.X, v.Y);
    public static implicit operator Vector(DVector2 v) => new(v.X, v.Y);
}

public class Quadratic
{
    public Quadratic(Line line, Vector offset)
        => (Line, Offset) = (line, offset);
    public Line Line { get; set; }
    public Vector Offset { get; set; }
    public Position Control
    {
        get => Line.A + Offset.ToVector();
        set => Offset = value.ToVector() - Line.A;
    }
}

public static class MathExtensions
{
    public static DVector2 MoveToDistanceFrom(this DVector2 self, DVector2 origin, double distance)
    {
        var v = self - origin;
        var n = v.Normalize();
        var sign = (v.X < 0 ? -1 : 1, v.Y < 0 ? -1 : 1);
        var n2 = n * sign;
        return origin + n2 * distance;
    }
}