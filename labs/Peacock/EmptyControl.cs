namespace Peacock;

public class EmptyView : IView
{
    public object Id => Guid.NewGuid();
    public static EmptyView Default = new();
}

public record EmptyControl() : Control<EmptyView>(
    new Measures(),
    EmptyView.Default,
    Array.Empty<IControl>(),
    DefaultCallback);
