using Ara3D.Collections;
using Ara3D.Mathematics;
using System.Windows;

namespace Ara3D.NodeEditor
{
    
    public abstract class ControlTemplate : IControlTemplate 
    {
        public abstract Control Create(IModel model, Control parent);

        public virtual StyleOptions StyleOptions => null;

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
    }

    public class GraphControl : ControlTemplate
    {
        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }
    }

    public class PropertyControl : ControlTemplate
    {
        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderControl : ControlTemplate
    {
        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }
    }

    public class FooterControl : ControlTemplate
    {
        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }
    }

    public class NodeControl : ControlTemplate
    {
        public HeaderControl HeaderControl { get; }
        public HeaderControl FootControl { get; }
        public OperatorControl OperatorControl { get; }
        public StyleOptions StyleOptions { get; }
        public IArray<IBehavior> DefaultBehaviors { get; }

        public NodeControl(OperatorControl opControl, StyleOptions options)
        {
            OperatorControl = opControl;
            StyleOptions = options;
        }

        public override Control Create(IModel model, Control parent)
        {
            var geometry = new Geometry(parent.Bounds);
            var view = new View(model, "Node", StyleOptions["Default"], geometry, null);
            var r = new Control(this, model, view, null, null);
            var children = CreateChildren(r);
            r = r with { Children = children };
            r = r.UpdateBounds(ComputeBounds(r));
            return r;
        }

        public IArray<Control> CreateChildren(Control parent)
        {
            var nodeModel = parent.Model as NodeModel;
            var opRect = parent.View.Geometry.BoundingRect;
            var controls = new List<Control>();
            for (var i = 0; i < nodeModel.Operators.Count; i++)
            {
                opRect = opRect.MoveBy(new Point(0, opRect.Height));
                var opModel = nodeModel.Operators[i];
                var control = OperatorControl.Create(opModel, opRect);
                controls.Add(control);
            }
            return controls.ToIArray();
        }
    }

    public class OperatorControl : ControlTemplate
    {
        public Control Create(IModel model, Rect rect)
        {
            throw new NotImplementedException();
        }

        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }

        public StyleOptions StyleOptions { get; }
        public IArray<Control> CreateChildren(Control parent)
        {
            throw new NotImplementedException();
        }
    }
}
