using System.IO;
using Ara3D.Collections;
using Svg;

namespace Ara3D.SVG.Creator;

public interface IEntity
{
    public SvgElement Svg { get; }
    public IEntity Clone();
}

public interface IEntityDescriptor
{
    public string Name { get; }
    public IEntity Create();
    public IEntity Read(Stream stream);
    public Stream Write(Stream stream);
}

public interface ICompound : IEntity
{
    IReadOnlyList<IEntity> Entities { get; }
}

public interface IElement : IEntity
{
    public int Index { get; set; }
}

public class SvgEntity : IElement
{
    public SvgElement Svg { get; set; }
    public int Index { get; set; }
    public IEntity Clone() => new SvgEntity() { Svg = Svg.DeepCopy() };
    public static SvgEntity Create(SvgElement svg) => new() { Svg = svg };
    public static SvgEntity Create(string txt) => new() { Svg = SvgDocument.FromSvg<SvgDocument>(txt) };
    public static implicit operator SvgEntity(string s) => Create(s);
    public static implicit operator SvgEntity(SvgElement e) => Create(e);
}

public class Compound : ICompound
{
    public IReadOnlyList<IEntity> Entities { get; } 

    public SvgElement Svg
    {
        get
        {
            var r = new SvgGroup();
            foreach (var x in Entities)
                r.Children.Add(x.Svg);
            return r;
        }
    }

    public Compound(IReadOnlyList<IEntity> entities)
        => Entities = entities;

    public IEntity Clone()
        => new Compound(Entities.Select(e => e.Clone()).ToList());
}

public class BoundEntity : IEntity
{

    public Operator Op { get; }
    public IEntity Target { get; }
    public SvgElement Svg { get; }
    public IEntity Clone()
    {
        throw new NotImplementedException();
    }
}

public static class EntityExtensions
{
    public static IElement ModifySvg(this IElement entity, Action<SvgElement> action)
    {
        action.Invoke(entity.Svg);
        return entity;
    }
    
}