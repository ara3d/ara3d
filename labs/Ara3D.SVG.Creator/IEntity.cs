using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Ara3D.Collections;
using Ara3D.Math;
using Svg;

namespace Ara3D.SVG.Creator;

public interface IEntity
{
    public SvgElement Svg { get; }
    public IEntity Clone();
}


/*
public interface IEntityDescriptor
{
    public string Name { get; }
    public IEntity Create();
    public IEntity Read(Stream stream);
    public Stream Write(Stream stream);
}
*/


public static class AttributeBufferShortNames
{
    public static Dictionary<string, string> ShortNames = new Dictionary<string, string>()
    {
        { "p", "position" },
        { "px", "position.x" },
        { "py", "position.y" },
        { "x", "position.x" },
        { "y", "position.y" },
        { "k", "skew" },
        { "kx", "skew.x" },
        { "ky", "skew.y" },
        { "s", "scale" },
        { "sx", "scale.x" },
        { "sy", "scale.y" },
        { "n", "normal" },
        { "nx", "normal.x" },
        { "ny", "normal.y" },
        { "t", "tangent" },
        { "tx", "tangent.x" },
        { "ty", "tangent.y" },
        { "r", "rotation" },
        { "i", "index" },
        { "t", "amount" },
        { "z", "size" },
        { "u", "uv.x" },
        { "v", "uv.y" },
    };
}

public interface ICompound : IEntity
{
    IArray<IEntity> Entities { get; }
}

public interface IElement : IEntity
{
}

public class SvgEntity : IElement
{
    public SvgElement Svg { get; set; }
    public IEntity Clone() => new SvgEntity() { Svg = Svg.DeepCopy() };
    public static SvgEntity Create(SvgElement svg) => new() { Svg = svg };
    public static SvgEntity LoadFromFile(string filePath) {
        var doc = SvgDocument.Open(filePath);
        var group = new SvgGroup();
        foreach (var child in  doc.Children)
        {
            group.Children.Add(child);
        }
        return group;   
    }
    public static SvgEntity Create(string txt) => new() { Svg = SvgDocument.FromSvg<SvgDocument>(txt) };
    public static implicit operator SvgEntity(string s) => Create(s);
    public static implicit operator SvgEntity(SvgElement e) => Create(e);
}

public static class EntityExtensions
{
    public static IEntity ModifySvg(this IEntity entity, Action<SvgElement> action)
    {
        action.Invoke(entity.Svg);
        return entity;
    }
    
}