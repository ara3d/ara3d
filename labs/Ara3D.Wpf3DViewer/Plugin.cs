using System;
using System.Linq;
using System.Reflection;
using Ara3D.Collections;
using Ara3D.Geometry;

namespace Wpf3DViewer;

public interface IProperty
{
    object Value { get; }
    IPropertyDescriptor Descriptor { get; }
}

public interface IPropertyDescriptor
{
    Type Type { get; }
    string Name { get; }
    string Description { get; }
    object Default { get; }
    Tuple<object, object> GetRange();
    object Update(object p, bool increase, bool largeAmount);
    object Validate(object p);
    object[] FixedValues { get; }
    string ToJson(object p);
    object FromJson(object p);
}

public interface IPropertyBlock
{
    void SetValue(object value);
}

public interface IPlugin
{
}

public interface IMeshPlugin : IPlugin
{
    IMesh Generate();
}

public class PropertyDescriptor : IPropertyDescriptor
{
    public PropertyDescriptor(Type type, string name = "", string description = "", object defaultValue = null)
    {
        Default = Activator.CreateInstance(Type);
    }

    public Type Type { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public object Default { get; set; } 

    public Tuple<object, object> GetRange() => null;
    public object Update(object p, bool increase, bool largeAmount) => p;
    public object Validate(object p) => p;
    public object[] FixedValues => Array.Empty<object>();

    public string ToJson(object p)
    {
        throw new NotImplementedException();
    }

    public object FromJson(object p)
    {
        throw new NotImplementedException();
    }

    public static PropertyDescriptor Create(PropertyInfo pi)
        => throw new NotImplementedException();
}

public class PluginData
{
    public Type PluginType { get; }
    public IPlugin PluginInstance { get; }
    public IPropertyDescriptor[] Descriptors { get; }
    public object[] PropertyValues { get; }

    public  PluginData(Type pluginType)
    {
        PluginType = pluginType;
        PluginInstance = Activator.CreateInstance(PluginType) as IPlugin;
        Descriptors = PluginType.GetProperties().Select(PropertyDescriptor.Create).ToArray();
    }
}


public class PluginRegistry
{
}