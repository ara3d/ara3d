namespace Ara3D.NodeEditor;

public record Control(
    IView View,
    IReadOnlyList<Control> Children,
    IReadOnlyList<IBehavior> Behaviors)
{
    public IModel Model
        => View.Model;

    public Guid Id
        => Model.Id;

    public static Control Create(IView view)
        => new(view, Array.Empty<Control>(), Array.Empty<IBehavior>());
}
