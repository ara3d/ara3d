using System.Windows;

namespace Ara3D.NodeEditor;

public record BaseView(IModel Model, Style Style, Rect Rect) : IView
{
    public virtual bool HitTest(Point point) => Rect.Contains(point);
    public virtual ICanvas Draw(ICanvas canvas) => canvas;
    public virtual IView Update(IView other) => this;
}

public record GraphView(IModel Model, Style Style, Rect Rect) : BaseView(Model, Style, Rect)
{
}

public record NodeView(IModel Model, Style Style, string Name, Rect Rect) : BaseView(Model, Style, Rect)
{
}

public record OperatorView(IModel Model, Style Style, string Text, Rect Rect) : BaseView(Model, Style, Rect)
{
}

public record PropertyView(IModel Model, Style Style, string Text, Rect Rect) : BaseView(Model, Style, Rect)
{
}

public record SocketView(IModel Model, Style Style, Rect Rect, bool LeftOrRight) : BaseView(Model, Style, Rect)
{
}

public record ConnectorView(IModel Model, Style Style, Point A, Point B) : BaseView(Model, Style, Rect.Empty)
{
}