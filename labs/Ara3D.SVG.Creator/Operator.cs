using System.Drawing;
using Ara3D.Math;
using Svg;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public abstract class Operator
{
    public abstract IEntity Evaluate(IEntity e, float strength);
}

public class SetStroke : Operator
{
    public StrokeWidth Width { get; set; } = 3f;

    public Color Color { get; set; } = Color.DarkSlateBlue;

    public override IEntity Evaluate(IEntity e, float strength)
        => e.ModifySvg(x =>
        {
            x.StrokeWidth += (float)(Width - x.StrokeWidth) * strength;
            if (x.Stroke is SvgColourServer cs)
                x.Stroke = new SvgColourServer(cs.Colour.Lerp(Color, strength));
            else
                x.Stroke = new SvgColourServer(Color);
        });
}

public class SetFillColor : Operator
{
    public Color Color { get; set; } = Color.Coral;

    public override IEntity Evaluate(IEntity e, float strength)
        => e.ModifySvg(x =>
        {
            if (x.Fill is SvgColourServer cs)
                x.Fill = new SvgColourServer(cs.Colour.Lerp(Color, strength));
            else
                x.Fill = new SvgColourServer(Color);
        });
}

public class TransformOperator : Operator
{
    public Vector Translation { get; set; } = DVector2.Zero;
    public Vector Skew { get; set; } = DVector2.Zero;
    public Angle Rotation { get; set; } = 0;
    public Scale Scale { get; set; } = DVector2.One;

    public override IEntity Evaluate(IEntity e, float strength)
        => e.ModifySvg(x =>
        {
            var tr = DVector2.Zero.Lerp(Translation.ToVector(), strength).Vector2;
            var sk = DVector2.Zero.Lerp(Skew.ToVector(), strength).Vector2;
            var ro = 0f.Lerp((float)Rotation.Degrees, strength);
            var sc = DVector2.One.Lerp(Scale.ToVector(), strength).Vector2;

            if (x.Transforms == null)
                x.Transforms = new SvgTransformCollection();

            x.Transforms.Add(new SvgTranslate(tr.X, tr.Y));
            x.Transforms.Add(new SvgRotate(ro));
            x.Transforms.Add(new SvgScale(sc.X, sc.Y));
            x.Transforms.Add(new SvgSkew(sk.X, sk.Y));
        });
}

public static class Extensions
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