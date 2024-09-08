namespace Ara3D.NodeEditor;

/// <summary>
/// A set of style options associated with a kind of control. 
/// </summary>
public class StyleOptions
{
    public Dictionary<string, IStyle> Styles = new();
    public IStyle? this[string name] => Styles.TryGetValue(name, out var result) ? result : null;
    public IStyle? Default => this["default"];
}