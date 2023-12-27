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
    public TransformViewModel Transform { get; set; } = new TransformViewModel();

    public override IEntity Evaluate(IElement e, float strength)
        => e.ModifySvg(x =>
        {
            var transform = Transform;
            if (strength.AlmostEquals(1))
                transform = new TransformViewModel().LerpTo(Transform, strength);

            if (x.Transforms == null)
                x.Transforms = new SvgTransformCollection();

            x.Transforms.Add(new SvgTranslate((float)transform.PositionX, (float)transform.PositionY));
            x.Transforms.Add(new SvgRotate((float)transform.RotationAngle));
            x.Transforms.Add(new SvgScale((float)transform.ScaleX, (float)transform.ScaleY));
            x.Transforms.Add(new SvgSkew((float)transform.SkewX, (float)transform.SkewY));
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