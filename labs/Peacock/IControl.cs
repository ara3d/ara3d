namespace Peacock;

/// <summary>
/// An immutable UI control. A control generates child controls on demand, but does not maintain a list. 
/// </summary>
public interface IControl
{
    IView View { get; }
    Measures Measures { get; }
    Func<IUpdates, IControl, IControl, IUpdates> Callback { get; }
    ICanvas Draw(ICanvas canvas);
    IReadOnlyList<IControl> Children { get; }
    IEnumerable<IBehavior> GetDefaultBehaviors();
    IUpdates Process(IInputEvent input, IUpdates updates);
}

public static class ControlExtensions
{
    public static IEnumerable<IControl> Descendants(this IControl control)
    {
        foreach (var child in control.Children)
            foreach (var d in child.Descendants())
                yield return d;
        yield return control;
    }
}