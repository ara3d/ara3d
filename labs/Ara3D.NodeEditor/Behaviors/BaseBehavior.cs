using Ara3D.Domo;

namespace Ara3D.NodeEditor;

public class BaseBehavior : IBehavior
{
    public virtual IBehavior Update(UserInput input, Control control)
        => this;

    public virtual Control Apply(Control control)
        => control;

    public virtual IModel ToModel(Control control)
        => control.View.Model;

    public virtual ICanvas PreDraw(ICanvas canvas, Control control)
        => canvas;

    public ICanvas PostDraw(ICanvas canvas, Control control)
        => canvas;
}