using Ara3D.Collections;
using Ara3D.Mathematics;
using Svg.Transforms;

namespace Ara3D.SVG.Creator;

public class AttributeBuffers  
{
    public IArray<Vector2> Position;
    public IArray<Vector2> Skew;
    public IArray<Vector2> Scale;
    public IArray<Vector2> Uv;
    public IArray<float> Index;
    public IArray<float> Amount;
    public IArray<float> Rotation;
    public IArray<float> Size;
    public IArray<float> Selection;
    public IArray<float> Depth;
    public IArray<float> Thickness;
    public IArray<ColorHDR> Stroke;
    public IArray<ColorHDR> Fill;

    public readonly int Count;
    
    public AttributeBuffers(int n)
    {
        Count = n;

        Position = Vector2.Zero.Repeat(n);
        Skew = Vector2.Zero.Repeat(n);
        Scale = Vector2.One.Repeat(n);
        Uv = Vector2.Zero.Repeat(n);
        Index = n.Select(i => (float)i);
        Amount = n.Select(i => (float)i / n);
        Rotation = 0f.Repeat(n);
        Size = 1f.Repeat(n);
        Selection = 0f.Repeat(n);
        Depth = 0f.Repeat(n);
        Thickness = 1f.Repeat(n);
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
            var tr = Position[i];
            var sk = Skew[i];
            var ro = (float)Rotation[i];
            var sc = Scale[i];
                
            x.Transforms ??= new SvgTransformCollection();

            x.Transforms.Add(new SvgTranslate(tr.X, tr.Y));
            x.Transforms.Add(new SvgRotate(ro));
            x.Transforms.Add(new SvgScale(sc.X, sc.Y));
            x.Transforms.Add(new SvgSkew(sk.X, sk.Y));
        });
    }

    public AttributeBuffers Transform(NumericAttributesEnum id, Func<AttributeBuffers, float, int, float> f)
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

    public AttributeBuffers Transform(VectorAttributesEnum id, Func<AttributeBuffers, Vector2, int, Vector2> f)
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
