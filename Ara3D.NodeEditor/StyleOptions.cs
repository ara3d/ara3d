namespace Ara3D.NodeEditor;

/// <summary>
/// A set of style options associated with a kind of control. 
/// </summary>
public class StyleOptions
{
    public Dictionary<string, Style> Styles = new();
    public Style this[string name] => Styles.TryGetValue(name, out var result) ? result : Style.Empty;
    public Style Default => this["default"];
}