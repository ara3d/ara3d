    namespace Ara3D.NodeEditor;

/// <summary>
/// A dictionary of values. 
/// </summary>
public class Style
{
    public static readonly Style Empty = new Style();
    public Dictionary<string, object> Properties = new();
}