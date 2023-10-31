using System.Windows;

namespace Peacock;

/// <summary>
/// This is a default implementation of IControl that can be used as-is, or also serve as a base class for other controls.
/// </summary>
public record Control<TView>(
    Measures Measures,
    TView View, 
    IReadOnlyList<IControl> Children, 
    Func<IUpdates, IControl, IControl, IUpdates> Callback) : IControl 
    where TView : IView
{
    public Control(Measures measures, TView view, Func<IUpdates, IControl, IControl, IUpdates> Callback)
        : this(measures, view, Array.Empty<IControl>(), Callback) { }

    IView IControl.View => View;
    public virtual ICanvas Draw(ICanvas canvas) => canvas;
    public virtual IUpdates Process(IInputEvent input, IUpdates updates) => updates;
    public virtual IEnumerable<IBehavior> GetDefaultBehaviors() => Enumerable.Empty<IBehavior>();
    public static IUpdates DefaultCallback(IUpdates updates, IControl oldControl, IControl newControl) => updates;

    public static IReadOnlyList<IControl> ToChildren(params IControl?[] controls)
        => controls.Where(c => c != null).OfType<IControl>().ToList();

    public static IReadOnlyList<IControl> ToChildren(params IEnumerable<IControl>[] controlLists)
        => controlLists.SelectMany(c => c).ToList();

    public static IControl Default = new EmptyControl();

    public Rect Relative => Measures.RelativeRect;
    public Rect Absolute => Measures.AbsoluteRect;
    public Rect Client => Measures.ClientRect;
    public Size Size => Measures.Size;
}
