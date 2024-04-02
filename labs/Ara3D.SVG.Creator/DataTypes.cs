using Ara3D.Mathematics;

namespace Ara3D.SVG.Creator;


public class Size
{
    public Size(float w, float h) => (W, H) = (w, h);
    public float W { get; set; }
    public float H { get; set; }
    public Vector2 ToVector() => (W, H);
    public static implicit operator Vector2(Size s) => new(s.W, s.H);
    public static implicit operator Size(Vector2 v) => new(v.X, v.Y);
}

public class Position
{
    public Position(float x, float y) => (X, Y) = (x, y);
    public float X { get; set; }
    public float Y { get; set; }
    public Vector2 ToVector() => (X, Y);
    public static implicit operator Vector2(Position p) => new(p.X, p.Y);
    public static implicit operator Position(Vector2 v) => new(v.X, v.Y);
}

public class StrokeWidth
{
    public float Amount { get; set; }
    public StrokeWidth(float x) => Amount = x;
    public static implicit operator StrokeWidth(float x) => new(x);
    public static implicit operator float(StrokeWidth x) => x.Amount;
}

public class Circle
{
    public Circle(Position center, float r)
        => (Center, R) = (center, r);
    public Position Center;
    public float R;
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

    public float L
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
    public Square(Position center, float l)
        => (Center, L) = (center, l);
    public Position Center { get; set; }
    public float L { get; set; }
};

public class Scale
{
    public Scale(float x, float y)
        => (X, Y) = (x, y);
    public Vector2 ToVector() => (X, Y);
    public float X { get; set; }
    public float Y { get; set; }
    public static implicit operator Vector2(Scale s) => new(s.X, s.Y);
    public static implicit operator Scale(Vector2 v) => new(v.X, v.Y);
};

public class Vector
{
    public Vector(float x, float y)
        => (X, Y) = (x, y);
    public float X { get; set; }
    public float Y { get; set; }
    public Vector2 ToVector() => (X, Y);
    public static implicit operator Vector2(Vector v) => new(v.X, v.Y);
    public static implicit operator Vector(Vector2 v) => new(v.X, v.Y);
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
    public static Vector2 MoveToDistanceFrom(this Vector2 self, Vector2 origin, float distance)
    {
        var v = self - origin;
        var n = v.Normalize();
        var sign = (v.X < 0 ? -1 : 1, v.Y < 0 ? -1 : 1);
        var n2 = n * sign;
        return origin + n2 * distance;
    }
}