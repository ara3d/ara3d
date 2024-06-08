using Ara3D.Collections;
using System.Windows;

namespace Ara3D.NodeEditor
{

    public record ControlPath(ControlPath Previous, Control Control);

    public class NodeEditorCoordinator : ICoordinator
    {

        public static Control SynchronizeControl(Control current, Control expected)
        {
            // Keeps the IBehavior
            // Add children if needed.
            // Removes children if needed. 
            // Updates children as needed. 
        }

        public static Point GetSocketPosition(Guid id)
        {
            // To-do: needs the GraphView, or at least the list of the nodes 
            // From there we need to create a dictionary of the sockets. 
            // How do I get the sockets from a model? 
            // This implies that creating a view needs the parent view.  
            throw new NotImplementedException();
        }

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
        

        public static IView CreateView(IModel model, IView parentView)
        {
            switch (model)
            {
                case GraphModel graphModel:
                    return new GraphView(graphModel, Style.Empty, graphModel.Rect);

                case ConnectorModel connectorModel:
                    return new ConnectorView(connectorModel, Style.Empty, GetSocketPosition(connectorModel.SourceId), GetSocketPosition(connectorModel.DestinationId));

                case NodeModel nodeModel:
                    return new NodeView(nodeModel, Style.Empty, nodeModel.Name, nodeModel.Rect);

                case OperatorModel operatorModel:
                    return new OperatorView(operatorModel, Style.Empty, operatorModel.Name, GetOperatorRect(operatorModel.Index));

                case PropertyModel propertyModel:
                    return new PropertyView(propertyModel, Style.Empty, propertyModel.Name, GetPropertyRect(propertyModel.Index));
            }

            throw new ArgumentOutOfRangeException(nameof(model));
        }

        public static void UpdateView(Control control, IView view)
        {
            // TODO: 
        }

        public static Control CreateControl(IModel model, Control parentControl)
        {
            var view = CreateView(model, null);
            var children = CreateChildViews(view, model);

        }

        public static IReadOnlyList<IView> CreateChildViews(IView parentView, IModel model)
        {
            foreach (var child in model.Children)
            {
                CreateView(child, parentView);
            }
        }

        public Control CreateControlTree(IModel model)
        {
            throw new NotImplementedException();
        }

        public Control SynchronizeControlTree(Control oldControl, Control newControl)
        {
            // Walks both trees, updating the 
            var syncTree = newControl;
            if (oldControl.Id != newControl.Id)
                throw new Exception("Attempting to synchronize different models");

            // Move the behaviors 
            syncTree = syncTree with { Behaviors = oldControl.Behaviors };
            
            // Move the views 
            syncTree = syncTree with { View = oldControl.View };

            // Create a new child list 
            var newChildren = new List<Control>();

            // Map the children to the new things 
            var lookup = oldControl.Children.ToDictionary(c => c.Id, c => c);

            foreach (var newChild in newControl.Children)
            {
                if (lookup.ContainsKey(newChild.Id))
                {
                    var oldChild = lookup[newChild.Id];
                    var syncChild = SynchronizeControl(oldChild, newChild);
                    newChildren.Add(syncChild);
                }
                else
                {
                    newChildren.Add(newChild);
                }
            }

            syncTree = syncTree with { Children = newChildren };
            return syncTree;
        }

        public Control UpdateControlFromModel(Control control, IModel model)
        {
            var newControl = CreateControl(model, )
            if (control == null)
            {

            }

            if (control.Id != model.Id)
            {
                throw new Exception("Control is not ")
            }
        }

        public IModel UpdateModelFromControl(Control control)
        {
            // TODO: 
            return control.Model;
        }
    }
}
