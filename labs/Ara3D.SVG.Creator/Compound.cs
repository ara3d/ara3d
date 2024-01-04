using Ara3D.Collections;
using Svg;

namespace Ara3D.SVG.Creator;

public class Compound : ICompound
{
    public IArray<IEntity> Entities { get; }
    public AttributeBuffers Buffers { get; }
    
    public SvgElement Svg
    {
        get
        {
            var r = new SvgGroup();
            for (var i = 0; i < Entities.Count; i++)
            {
                var e = Entities[i];
                e = Buffers.Transform(e, i);
                r.Children.Add(e.Svg);
            }

            return r;
        }
    }

    public Compound(IArray<IEntity> entities)
        : this(entities, new AttributeBuffers(entities.Count))
    { }

    public Compound(IArray<IEntity> entities, AttributeBuffers buffers)
        => (Entities, Buffers) = (entities, buffers.Clone());

    public IEntity Clone()
        => new Compound(Entities.Select(e => e.Clone()), Buffers);

    public Compound With(Func<AttributeBuffers, AttributeBuffers> transform)
        => new(Entities, transform(Buffers));
}