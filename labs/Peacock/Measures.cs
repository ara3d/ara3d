using System.Windows;

namespace Peacock;

/// <summary>
/// The absolute position,
/// </summary>
public record Measures(Point ParentPosition, Vector Offset, Size Size)
{
    public Measures(Point Parent, Rect OffsetRect)
        : this(Parent, new Vector(OffsetRect.Left, OffsetRect.Top), OffsetRect.Size) 
    { }

    public Measures(Measures parent, Vector offset, Size size)
        : this(parent.ParentPosition + offset, offset, size)
    { }

    public Measures(Measures parent, Vector offset)
        : this(parent, offset, parent.Size) { }

    public Measures()
        : this(new Point(), new Vector(0, 0), Size.Empty)
    { }

    public Point AbsolutePosition
        => ParentPosition + Offset;

    public Rect AbsoluteRect
        => new(AbsolutePosition, Size);

    public Rect RelativeRect
        => new(new Point(Offset.X, Offset.Y), Size);

    public Rect ClientRect
        => new(Size);

    public Measures Relative(Size size)
        => Relative(new Vector(0,0), size);

    public Measures Relative(Vector offset, Size size)
        => new(AbsolutePosition, offset, size);

    public Measures Relative(Rect rect)
        => Relative(new Vector(rect.Left, rect.Top), rect.Size);

    public Measures Relative(Vector offset)
        => Relative(offset, Size);

    public Measures Relative()
        => Relative(new Vector(0,0));

    public static Measures Default = new();
}