namespace Ara3D.SVG.Creator;

public class UIAttribute : Attribute
{
}

public class UINameAttribute : UIAttribute
{
    public UINameAttribute(string name) => Name = name;
    public string Name { get; }
}

public class UIChangeAttribute : UIAttribute
{
    public UIChangeAttribute(double change) => Change = change;
    public double Change { get; }
}
