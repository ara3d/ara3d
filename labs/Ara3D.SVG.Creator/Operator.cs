using System.Drawing;
using Ara3D.Math;
using Svg;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public abstract class Operator
{
    public abstract IEntity Evaluate(IElement e, float strength);

    // TODO: this should only be shown if the object is a clone. 
    public bool LinearRamp { get; set; }
}

public class SetStrokeWidth : Operator
{
    public float Width { get; set; }

    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x => x.StrokeWidth += (Width - x.StrokeWidth) * strength);
}

public class SetFillColor : Operator
{
    public Color Color { get; set; }

    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x =>
        {
            if (x.Fill is SvgColourServer cs)
                x.Fill = new SvgColourServer(cs.Colour.Lerp(Color, strength));
            else
                x.Fill = new SvgColourServer(Color);
        });
}

public class SetStrokeColor : Operator
{
    public Color Color { get; set; }

    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x =>
        {
            if (x.Stroke is SvgColourServer cs)
                x.Stroke = new SvgColourServer(cs.Colour.Lerp(Color, strength));
            else
                x.Stroke = new SvgColourServer(Color);
        });
}

public class TransformOperator : Operator
{
    public Vector Translation { get; set; } = DVector2.Zero;
    public Vector Skew { get; set; } = DVector2.Zero;
    public Angle Rotation { get; set; } = 0;
    public Scale Scale { get; set; } = DVector2.One;

    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x =>
        {
            var tr = DVector2.Zero.Lerp(Translation.ToVector(), strength).Vector2;
            var sk = DVector2.Zero.Lerp(Skew.ToVector(), strength).Vector2;
            var ro = (float)0.0.Lerp(Rotation.Degrees, strength);
            var sc = DVector2.One.Lerp(Scale.ToVector(), strength).Vector2;

            if (x.Transforms == null)
                x.Transforms = new SvgTransformCollection();

            x.Transforms.Add(new SvgTranslate(tr.X, tr.Y));
            x.Transforms.Add(new SvgRotate(ro));
            x.Transforms.Add(new SvgScale(sc.X, sc.Y));
            x.Transforms.Add(new SvgSkew(sk.X, sk.Y));
        });

}
/*
public abstract class TransformOperator : Operator
{
    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x =>
        {
            if (x.Transforms == null)
                x.Transforms = new SvgTransformCollection();
            x.Transforms.Add(GetTransform(strength));
        });

    public abstract SvgTransform GetTransform(float strength);
}

public class Rotate : TransformOperator
{
    public float Angle { get; set; }
    public Vector2 Center { get; set; }

    public override SvgTransform GetTransform(float strength)
        => new SvgRotate(0f.Lerp(Angle, strength), Center.X, Center.Y);
}

public class Scale : TransformOperator
{
    public Vector2 Amount { get; set; }

    public override SvgTransform GetTransform(float strength)
        => new SvgScale(Vector2.One.Lerp(Amount, strength).X, Vector2.One.Lerp(Amount, strength).Y);
}

public class ScaleUniform : TransformOperator
{
    public float Amount { get; set; }

    public override SvgTransform GetTransform(float strength)
        => new SvgScale(1f.Lerp(Amount, strength));
}

public class Skew : TransformOperator
{
    public Vector2 Amount { get; set; }

    public override SvgTransform GetTransform(float strength)
        => new SvgSkew(Vector2.Zero.Lerp(Amount, strength).X, Vector2.Zero.Lerp(Amount, strength).Y);
}

public class Translate : TransformOperator
{
    public Vector2 Amount { get; set; }

    public override SvgTransform GetTransform(float strength)
        => new SvgTranslate(Vector2.Zero.Lerp(Amount, strength).X, Vector2.Zero.Lerp(Amount, strength).Y);
}
*/

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

    public static IEntity Evaluate(this Operator op, IEntity e)
        => Evaluate(op, e, 1f);

    public static IEntity Evaluate(this Operator op, IEntity e, float strength)
    {
        if (e is ICompound compound)
        {
            if (op.LinearRamp)
            {
                return new Compound(compound.Entities
                    .Select((e1, i) => op.Evaluate(e1, (float)i / compound.Entities.Count)).ToList());
            }
            else
            {
                return new Compound(compound.Entities
                    .Select((e1, i) => op.Evaluate(e1)).ToList());
            }
        }
        else
        {
            return op.Evaluate((IElement)e, strength);
        }
    }
}