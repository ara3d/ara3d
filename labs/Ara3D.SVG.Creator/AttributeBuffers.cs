using Ara3D.Collections;
using Ara3D.Mathematics;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public class AttributeBuffers  
{
    public IArray<DVector2> Position;
    public IArray<DVector2> Skew;
    public IArray<DVector2> Scale;
    public IArray<DVector2> Uv;
    public IArray<double> Index;
    public IArray<double> Amount;
    public IArray<double> Rotation;
    public IArray<double> Size;
    public IArray<double> Selection;
    public IArray<double> Depth;
    public IArray<double> Thickness;
    public IArray<ColorHDR> Stroke;
    public IArray<ColorHDR> Fill;

    public readonly int Count;
    
    public AttributeBuffers(int n)
    {
        Count = n;

        Position = DVector2.Zero.Repeat(n);
        Skew = DVector2.Zero.Repeat(n);
        Scale = DVector2.One.Repeat(n);
        Uv = DVector2.Zero.Repeat(n);
        Index = n.Select(i => (double)i);
        Amount = n.Select(i => (double)i / n);
        Rotation = 0.0.Repeat(n);
        Size = 1.0.Repeat(n);
        Selection = 0.0.Repeat(n);
        Depth = 0.0.Repeat(n);
        Thickness = 1.0.Repeat(n);
        Stroke = new ColorHDR(1f, 1f, 1f, 1f).Repeat(n);
        Fill = new ColorHDR(1f, 1f, 1f, 1f).Repeat(n);
    }

    public IEntity Transform(IEntity entity, int i)
    {
        return entity.ModifySvg(x =>
        {
            x.StrokeWidth = (float)Thickness[i];
            //x.Stroke = new SvgColourServer(Stroke[i].ToSvg());
            //x.Fill = new SvgColourServer(Fill[i].ToSvg());
            var tr = Position[i].Vector2;
            var sk = Skew[i].Vector2;
            var ro = (float)Rotation[i];
            var sc = Scale[i].Vector2;
                
            x.Transforms ??= new SvgTransformCollection();

            x.Transforms.Add(new SvgTranslate(tr.X, tr.Y));
            x.Transforms.Add(new SvgRotate(ro));
            x.Transforms.Add(new SvgScale(sc.X, sc.Y));
            x.Transforms.Add(new SvgSkew(sk.X, sk.Y));
        });
    }

    public AttributeBuffers Transform(NumericAttributesEnum id, Func<AttributeBuffers, double, int, double> f)
    {
        var r = Clone();
        switch (id)
        {
            case NumericAttributesEnum.Position_X: 
                r.Position = r.Position.Select((v, i) => v.SetX(f(this, v.X, i)));
                break;
            case NumericAttributesEnum.Position_Y:
                r.Position = r.Position.Select((v, i) => v.SetY(f(this, v.Y, i)));
                break;
            case NumericAttributesEnum.Skew_X:
                r.Position = r.Skew.Select((v, i) => v.SetX(f(this, v.X, i)));
                break;
            case NumericAttributesEnum.Skew_Y:
                r.Position = r.Skew.Select((v, i) => v.SetY(f(this, v.Y, i)));
                break;
            case NumericAttributesEnum.Scale_X:
                r.Position = r.Scale.Select((v, i) => v.SetX(f(this, v.X, i)));
                break;
            case NumericAttributesEnum.Scale_Y:
                r.Position = r.Scale.Select((v, i) => v.SetY(f(this, v.Y, i)));
                break;
            case NumericAttributesEnum.UV_X:
                r.Position = r.Uv.Select((v, i) => v.SetX(f(this, v.X, i)));
                break;
            case NumericAttributesEnum.UV_Y:
                r.Position = r.Uv.Select((v, i) => v.SetY(f(this, v.Y, i)));
                break;
            case NumericAttributesEnum.Index:
                r.Index = r.Index.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Amount:
                r.Amount = r.Amount.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Rotation:
                r.Rotation = r.Rotation.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Size:
                r.Index = r.Index.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Selection:
                r.Selection = r.Selection.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Depth:
                r.Depth = r.Depth.Select((x, i) => f(this, x, i));
                break;
            case NumericAttributesEnum.Thickness:
                r.Thickness = r.Thickness.Select((x, i) => f(this, x, i));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }

        return r;
    }

    public AttributeBuffers Transform(VectorAttributesEnum id, Func<AttributeBuffers, DVector2, int, DVector2> f)
    {
        var r = Clone();
        switch (id)
        {
            case VectorAttributesEnum.Position:
                r.Position = r.Position.Select((x, i) => f(this, x, i));
                break;
            case VectorAttributesEnum.Skew:
                r.Skew = r.Skew.Select((x, i) => f(this, x, i));
                break;
            case VectorAttributesEnum.Scale:
                r.Scale = r.Scale.Select((x, i) => f(this, x, i));
                break;
            case VectorAttributesEnum.UV:
                r.Uv = r.Uv.Select((x, i) => f(this, x, i));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }

        return r;
    }

    public AttributeBuffers Transform(ColorAttributesEnum id, Func<AttributeBuffers, ColorHDR, int, ColorHDR> f)
    {
        var r = Clone();
        switch (id)
        {
            case ColorAttributesEnum.Fill:
                r.Fill = r.Fill.Select((x, i) => f(this, x, i));
                break;
            case ColorAttributesEnum.Stroke:
                r.Stroke = r.Stroke.Select((x, i) => f(this, x, i));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }

        return r;
    }

    public AttributeBuffers Clone()
    {
        return MemberwiseClone() as AttributeBuffers;
    }
}

public enum NumericAttributesEnum
{
    Position_X,
    Position_Y,
    Skew_X,
    Skew_Y,
    Scale_X,
    Scale_Y,
    UV_X,
    UV_Y,
    Index,
    Amount,
    Rotation, 
    Size, 
    Selection,
    Depth,
    Thickness,
}

public enum VectorAttributesEnum
{
    Position,
    Skew,
    Scale, 
    UV,
}

public enum ColorAttributesEnum
{
    Stroke,
    Fill,
}
