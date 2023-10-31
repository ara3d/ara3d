using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Peacock;

[Mutable]
public class SvgCanvas : ICanvas
{
    public StringBuilder Text = new StringBuilder();

    public override string ToString()
    {
        return Text + "</svg>";
    }

    public SvgCanvas(int width, int height)
    {
        Text.AppendLine($"<svg width='{width}' height='{height}'>");
    }

    public ICanvas Draw(StyledText text)
    {
        // TODO: compute the X and Y based on the alignment.
        Text.AppendLine($"<text x='{text.Rect.Left}' y='{text.Rect.Top}' style='{SvgStyle(text.Style)}'>{text.Text}</text>");
        return this;
    }

    public ICanvas Draw(StyledLine line)
    {
        return this;
    }

    public ICanvas Draw(StyledEllipse ellipse)
    {
        Text.AppendLine($"<ellipse {SvgEllipse(ellipse.Ellipse)} style='{SvgStyle(ellipse.Style)}' />");
        return this;
    }

    public static string SvgColor(Color color)
        => $"rgb({color.R}, {color.G}, {color.B})";

    public static string SvgStyle(ShapeStyle style)
        => $"fill: {SvgColor(style.BrushStyle.Color)}; " +
           $"stroke: {SvgColor(style.PenStyle.BrushStyle.Color)}; " +
           $"stroke - width:{style.PenStyle.Width}; " +
           $"fill - opacity:{style.BrushStyle.Color.A}; " +
           $"stroke - opacity:{style.PenStyle.BrushStyle.Color.A}";

    public static string SvgStyle(TextStyle style)
        => $"fill: {SvgColor(style.BrushStyle.Color)}; " +
           $"font-family: {style.FontFamily}; " +
           $"font-size: {style.FontSize}px; ";

    public static string SvgEllipse(Ellipse ellipse)
        => $"cx='{ellipse.Point.X}' cy='{ellipse.Point.Y}' rx='{ellipse.Radius.X}' ry='{ellipse.Radius.Y}'";

    public static string SvgRect(Rect rect)
        => $"x='{rect.Left}' y='{rect.Top}' width='{rect.Width}' height='{rect.Height}'";

    public ICanvas Draw(StyledRect rect)
    {
        Text.AppendLine($"<rect {SvgRect(rect.Rect.Rect)} style='{SvgStyle(rect.Style)}' />");
        return this;
    }

    public ICanvas Draw(BrushStyle brushStyle, PenStyle penStyle, Geometry geometry)
    {
        // TODO: draw none SVG elements
        return this;
    }

    public Size MeasureText(StyledText text)
    {
        // TODO: this will have to be JavaScript. Or we remove MeasureText
        return new(100, 20);
    }

    public ICanvas SetRect(Rect rect)
    {
        Text.AppendLine($"<g transform=\"translate({rect.Left} {rect.Top})\">");
        return this;
    }

    public ICanvas PopRect()
    {
        Text.AppendLine("</g>");
        return this;
    }
}