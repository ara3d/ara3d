using System.Windows;
using System.Windows.Media;

namespace Ara3D.NodeEditor;

public class ControlStyle : IStyle
{
    public Color BorderColor = Colors.BlueViolet;
    public Color FillColor = Colors.AntiqueWhite;
    public float Margin = 2;
    public float Border = 2;
    public float Padding = 2;
    public float FontSize = 9;
    public string FontName = "Arial";
    public ShapeStyle ShapeStyle => new(FillColor, new PenStyle(BorderColor, Border));
    public StyledRect GetStyledControlRect(Rect rect) => new(ShapeStyle, GetBorderRect(rect));
    public Rect GetBorderRect(Rect rect) => rect.Shrink(Margin);
    public Rect GetContentRect(Rect rect) => rect.Shrink(Margin + Border + Padding);
}

public class BaseView<TStyle> : IView
    where TStyle : ControlStyle, new()
{
    public BaseView(IModel model, string text, TStyle? style, Rect? rect)
    {
        Model = model;
        Text = text;
        Style = style ?? new();
        Rect = rect ?? Rect.Empty;
    }

    public IModel Model { get; }
    public string Text { get; }
    public TStyle Style { get; }
    IStyle IView.Style => Style;
    public Rect Rect { get; }
    public virtual bool HitTest(Point point) => Rect.Contains(point);
    public virtual ICanvas Draw(ICanvas canvas) => canvas;

    public Rect BorderRect => Style.GetBorderRect(Rect);
    public StyledRect StyledControlRect => new(Style.ShapeStyle, BorderRect);
    public Rect ContentRect => Style.GetContentRect(Rect);
}

public class GraphStyle : ControlStyle
{
}

public class GraphView : BaseView<GraphStyle>
{
    public GraphView(IModel model, GraphStyle? style, Rect? rect)
        : base(model, "", style, rect)
    {
    }
}

public class NodeStyle : ControlStyle
{
}

public class  NodeView : BaseView<NodeStyle>
{
    public NodeView(IModel model, string name, NodeStyle? style = null, Rect? rect = null)
        : base(model, name, style, rect)
    {
    }

    public override ICanvas Draw(ICanvas canvas)
    {
        return canvas.Draw(new StyledRect(Style.ShapeStyle, Rect));
    }
}

public class OperatorStyle : ControlStyle
{
}

public class OperatorView : BaseView<OperatorStyle>
{
    public OperatorView(IModel model, string text, OperatorStyle? style = null, Rect? rect = null)
        : base(model, text, style, rect)
    {
    }
}

public class PropertyStyle : ControlStyle
{
}

public class PropertyView : BaseView<PropertyStyle>
{
    public PropertyView(IModel model, string text, PropertyStyle? style = null, Rect? rect = null) 
        : base(model, text, style, rect)
    { }

}

public class SocketStyle : ControlStyle
{
}

public class SocketView : BaseView<SocketStyle>
{
    public bool LeftOrRight
    {
        get;
    }

    public SocketView(IModel model, bool leftOrRight, SocketStyle? style = null, Rect? rect = null)
        : base(model, "", style, rect)
    {
        LeftOrRight = leftOrRight;
    }
}

public class ConnectorStyle : ControlStyle
{
}

public class ConnectorView
    : BaseView<ConnectorStyle>
{
    public Point A { get; }
    public Point B { get; }

    public ConnectorView(IModel model, Point a, Point b, ConnectorStyle? style = null, Rect? rect = null)
        : base(model, "", style, rect)
    {
        A = a;
        B = b;
    }
}