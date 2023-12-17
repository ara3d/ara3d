using System.Drawing;
using Ara3D.Math;
using Svg;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public abstract class Modifier
{
    public SvgElement Update(SvgElement e) => Update(e, 1);
    public abstract SvgElement Update(SvgElement e, float strength);

    // TODO: this should only be shown if the object is clone. 
    public bool LinearRamp { get; set; }
}

public class SetStrokeWidth : Modifier
{
    public float Width { get; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var r = e.DeepCopy();
        var width = e.StrokeWidth + (Width - e.StrokeWidth) * strength;
        r.StrokeWidth = width;
        return r;
    }
}

public class SetFillColor : Modifier
{
    public Color Color { get; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var color = Color;
        if (e.Fill is SvgColourServer cs)
            color = cs.Colour.Lerp(Color, strength);
        e.Fill = new SvgColourServer(color);
        return e;
    }
}

public class SetStrokeColor : Modifier
{
    public Color Color { get; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var color = Color;
        if (e.Stroke is SvgColourServer cs)
            color = cs.Colour.Lerp(Color, strength);
        e.Stroke = new SvgColourServer(color);
        return e;
    }
}

public class Rotate : Modifier
{
    public float Angle { get; set; }
    public Vector2 Center { get; set; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var angle = Angle / strength;
        var r = e.DeepCopy();
        r.Transforms.Add(new SvgRotate(angle, Center.X, Center.Y));
        return r;
    }
}

public class Scale : Modifier
{
    public Vector2 Amount { get; set; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var amount = Vector2.One.Lerp(Amount, strength);
        var r = e.DeepCopy();
        r.Transforms.Add(new SvgScale(amount.X, amount.Y));
        return r;
    }
}

public class Skew : Modifier
{
    public Vector2 Amount { get; set; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var amount = Vector3.Zero.Lerp(Amount, strength);
        var r = e.DeepCopy();
        r.Transforms.Add(new SvgSkew(amount.X, amount.Y));
        return r;
    }
}

public class Translate : Modifier
{
    public Vector2 Amount { get; set; }

    public override SvgElement Update(SvgElement e, float strength)
    {
        var amount = Vector2.Zero.Lerp(Amount, strength);
        var r = e.DeepCopy();
        r.Transforms.Add(new SvgTranslate(amount.X, amount.Y));
        return r;
    }
}

public static class Exntesions
{
    public static float Lerp(this float from, float to, float strength)
    {
        return from + (to - from) * strength;
    }

    public static byte Lerp(this byte from, byte to, float strength)
    {
        return (byte)(from + ((float)to - (float)from) * strength);
    }

    public static Color Lerp(this Color from, Color to, float strength)
    {
        return Color.FromArgb(
            from.A.Lerp(to.A, strength),
            from.R.Lerp(to.R, strength),
            from.G.Lerp(to.G, strength),
            from.B.Lerp(to.B, strength));
    }
}