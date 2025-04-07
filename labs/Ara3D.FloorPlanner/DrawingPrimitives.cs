using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Plato;
using Color = System.Windows.Media.Color;

namespace Ara3D.FloorPlanner;

public enum FontWeight
{
    Normal,
    Bold,
}

public record Alignment(AlignmentX X, AlignmentY Y)
{
    public static Alignment LeftCenter => new(AlignmentX.Left, AlignmentY.Center);
    public static Alignment CenterCenter => new(AlignmentX.Center, AlignmentY.Center);
    public static Alignment RightCenter => new(AlignmentX.Right, AlignmentY.Center);
    public static Alignment LeftTop => new(AlignmentX.Left, AlignmentY.Top);
    public static Alignment CenterTop => new(AlignmentX.Center, AlignmentY.Top);
    public static Alignment RightTop => new(AlignmentX.Right, AlignmentY.Top);
    public static Alignment LeftBottom => new(AlignmentX.Left, AlignmentY.Bottom);
    public static Alignment CenterBottom => new(AlignmentX.Center, AlignmentY.Bottom);
    public static Alignment RightBottom => new(AlignmentX.Right, AlignmentY.Bottom);
}

public record WindowProps(Rect2D Rect, string Title, Cursor Cursor);

public record BrushStyle(Color Color) 
{
    public static implicit operator BrushStyle(Color color) => new(color);
    public static BrushStyle Empty = Colors.Transparent;
}

public record PenStyle(BrushStyle BrushStyle, float Width) 
{
    public static implicit operator PenStyle(Color color) => new(color, 1);
    public static implicit operator PenStyle(BrushStyle brush) => new(brush, 1);
    public static PenStyle Empty = Colors.Transparent;
}

public record TextStyle(BrushStyle BrushStyle, string FontFamily, FontWeight Weight, float FontSize, Alignment Alignment);
public record ShapeStyle(BrushStyle BrushStyle, PenStyle PenStyle) ;
public record StyledText(TextStyle Style, Rect2D Rect, string Text);    
public record StyledLine(PenStyle PenStyle, Line2D Line);
public record StyledRect(ShapeStyle Style, Rect2D Rect);
public record StyledEllipse(ShapeStyle Style, Ellipse Ellipse);

