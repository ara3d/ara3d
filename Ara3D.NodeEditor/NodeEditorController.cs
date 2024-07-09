using System.Windows;

namespace Ara3D.NodeEditor
{
    public class NodeEditorController : IController
    {
        public List<ControllerState> States { get; } 
            = new() { new ControllerState() };

        public ControllerState CurrentState 
            => States[States.Count - 1];

        public static Point GetSocketPosition(ControllerState state, Guid id)
            => state.Views[id].Rect.Center();

        public static Rect GetOperatorRect(int index)
        {
            // Todo: how do I get a pointer to the parent? 
            throw new NotImplementedException();
        }

        public static Rect GetPropertyRect(int index)
        {
            // Todo: how do I get a pointer to the parent? 
            throw new NotImplementedException();
        }

        public static ControllerState UpdateState(ControllerState state, Control control)
        {
            state.Controls.Add(control.Model.Id, control);
            state.Models.Add(control.Model.Id, control.Model);
            state.Views.Add(control.View.Model.Id, control.View);
            foreach (var child in control.Children)
                UpdateState(state, child);
            return state;
        }
        
        public Control? FindChildControl(Control? parent, IModel model) 
            => parent?.Children.FirstOrDefault(c => c.Model.Id == model.Id);

        public Control CreateControl(IView view)
        {
            // TODO: add the default behaviors. 
            return Control.Create(view);
        }

        public Control CreateControl(Control? prevControl, IModel model)
        {
            var view = CreateView(model, prevControl?.View);
            prevControl ??= Control.Create(view);
            var children = CreateChildren(prevControl, model.Children);
            return prevControl with { View = view, Children = children };
        }

        public IReadOnlyList<Control> CreateChildren(Control control, IEnumerable<IModel> models)
            => models.Select(child => CreateControl(FindChildControl(control, child), child)).ToList();

        public IView CreateView(IModel model, IView? previousView = null)
        {
            switch (model)
            {
                case GraphModel graphModel:
                    return CreateView(graphModel, previousView as GraphView);
                case ConnectorModel connectorModel:
                    return CreateView(connectorModel, previousView as ConnectorView);
                case NodeModel nodeModel:
                    return CreateView(nodeModel, previousView as NodeView);
                case OperatorModel operatorModel:
                    return CreateView(operatorModel, previousView as OperatorView);
                case PropertyModel propertyModel:
                    return CreateView(propertyModel, previousView as PropertyView);
                default:
                    throw new ArgumentOutOfRangeException(nameof(model));
            }
        }

        public GraphView CreateView(GraphModel model, GraphView? view = null)
        {
            return new GraphView(model, view?.Style, model.Rect);
        }

        public NodeView CreateView(NodeModel model, NodeView? view = null)
        {
            return new NodeView(model, model.Name, view?.Style, view?.Rect ?? model.Rect);
        }

        public OperatorView CreateView(OperatorModel model, OperatorView? view = null)
        {
            return new OperatorView(model, model.Name, view?.Style, view?.Rect);
        }

        public PropertyView CreateView(PropertyModel model, PropertyView? view = null)
        {
            return new PropertyView(model, model.Name, view?.Style, view?.Rect);
        }

        public SocketView CreateView(SocketModel model, SocketView? view = null)
        {
            return new SocketView(model, model.Position == 0, view?.Style, view?.Rect);
        }

        public IModel CreateModel(Control control)
        {
            return control.Model;
            /*
            // TODO: this needs to be implemented. 
            if (!(control.Model is GraphModel graphModel))
                throw new Exception("Expected a graph model");

            var r = graphModel;

            foreach (var child in control.Children)
            {
                if (child.Model is NodeModel nodeModel)
                {
                    foreach (var opControl in child.Children)
                    {
                        if (!(opControl.Model is OperatorModel opModel))
                            throw new Exception("Expected an operator model");

                        foreach (var propControl in opControl.Children)
                        {
                            if (!(propControl.Model is PropertyModel propertyModel))
                                throw new Exception("Expected a property model");

                            // TODO:
                        }
                    }
                }
                else if (child.Model is ConnectorModel connectorModel)
                {
                    throw new Exception("Expected connector model");
                }
                else
                {
                    throw new Exception("Expected a node or connector");
                }
            }

            return r;
            */
        }

        public IReadOnlyList<IBehavior> NewBehaviors(UserInput input, Control control)
        {
            if (control.Model is NodeModel nodeModel)
            {
                var rect = control.View.Rect;
            }

            throw new NotImplementedException();
        }
    }
}
