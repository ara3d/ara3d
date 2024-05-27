using Ara3D.Collections;
using System.Windows;

namespace Ara3D.NodeEditor
{
    public class GraphControl : ControlTemplate
    {
        public NodeControl Node { get; }
        public ConnectorControl Connector { get; }
        public GraphControl(StyleOptions styleOptions) : base(styleOptions)
        {
            Node = new NodeControl(styleOptions);
            Connector = new ConnectorControl(styleOptions);
        }
        
        public override Control Create(IModel model, Control parent)
        {
            return new Control(this, 
                new View(model, "Node Graph", StyleOptions.Default, null, null), 
                CreateChildren(parent), 
                LinqArray.Empty<IBehavior>());
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }

        public override IArray<Control> CreateChildren(Control parent)
        {
            var model = (GraphModel)parent.View.Model;
            var nodes = model.Nodes.Select(n => Node.Create(n, parent));
            var connectors = model.Connectors.Select(c => Connector.Create(c, parent));
            return nodes.Concat(connectors);
        }
    }

    public class PropertyControl : ControlTemplate
    {
        public PropertyControl(StyleOptions options)
            : base(options)
        { }

        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderControl : ControlTemplate
    {
        public HeaderControl(StyleOptions options)
            : base(options)
        { }

        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }
    }

    public class FooterControl : ControlTemplate
    {
        public FooterControl(StyleOptions options)
            : base(options)
        {
        }

        public override Control Create(IModel model, Control parent)
        {
            throw new NotImplementedException();
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }
    }

    public class NodeControl : ControlTemplate
    {
        public readonly HeaderControl Header;
        public readonly FooterControl Footer;
        public readonly OperatorControl Operator;
        public IArray<IBehavior> DefaultBehaviors = LinqArray.Empty<IBehavior>();

        public NodeControl(StyleOptions options)
            : base(options)
        {
            Header = new HeaderControl(options);
            Footer = new FooterControl(options);
            Operator = new OperatorControl(options);
        }

        public override Control Create(IModel model, Control parent)
        {
            var geometry = new Geometry(parent.Bounds);
            var view = new View(model, "Node", StyleOptions["Default"], geometry, null);
            var r = Control.Create(this, view);
            var children = CreateChildren(r);
            r = r with { Children = children };
            r = r.UpdateBounds(ComputeBounds(r));
            return r;
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
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
                var control = Operator.Create(opModel, opRect);
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

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }

        public StyleOptions StyleOptions { get; }
        public IArray<Control> CreateChildren(Control parent)
        {
            throw new NotImplementedException();
        }

        public OperatorControl(StyleOptions styleOptions) : base(styleOptions)
        {
        }
    }

    public class ConnectorControl : ControlTemplate
    {
        public ConnectorControl(StyleOptions options)
            : base(options)
        { }

        public override Control Create(IModel model, Control parent)
        {
            //return new Control(this, model, new View(model))
            throw new NotImplementedException();
        }

        public override Geometry ComputeGeometry(Control control)
        {
            throw new NotImplementedException();
        }
    }
}
