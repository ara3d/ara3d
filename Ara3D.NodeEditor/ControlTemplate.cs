using System.Windows;
using Ara3D.Collections;

namespace Ara3D.NodeEditor;


// A control template is both a factory for controls, and it manages the behavior.
// Controls are just state containers. 


public abstract class ControlTemplate : IControlTemplate
{
    // This is the binding (View from Model, Model from View)
    // TODO: provide these to the constructor.
    // TODO: determine if more data is going to be required.
    // There might need to be interface functions for them. 
    public Func<Control, IModel> UpdateModelFunc;
    public Func<Control, IModel, Control> UpdateControlFunc;
    public Func<Control, IArray<Control>> CreateChildrenFunc;

    protected ControlTemplate(StyleOptions styleOptions)
        => StyleOptions = styleOptions;

    public abstract Control Create(IModel model, Control parent);
        
    // TODO: this might need the entire context of the control. 
    public abstract Geometry ComputeGeometry(Control control);
            
    public StyleOptions StyleOptions { get; }

    public virtual Rect ComputeBounds(Control control)
    {
        var rect = control.View.Geometry.Position.ToRect(new Size(0,0));

        if (control.Children.Count == 0)
            return Rect.Empty;
            
        for (var i = 1; i < control.Children.Count; i++)
        {
            var child = control.Children[i];
            rect.Union(child.Bounds);
        }

        return rect;
    }

    public virtual IArray<Control> CreateChildren(Control parent)
        => LinqArray.Empty<Control>();

    public virtual ICanvas PreDraw(ICanvas canvas, Control control)
        => canvas;

    public virtual ICanvas PostDraw(ICanvas canvas, Control control)
        => canvas;

    public Control CreateDefailtControl(View view)
        => Control.Create(this, view);
}