using Ara3D.Collections;

namespace Ara3D.NodeEditor
{
    public class Model : IModel
    {
        public Guid Id { get; }
        public Model(Guid id)
            => Id = id;
    }

    public class PropertyModel : Model
    {
        public string Name { get; }
        public string Category { get; }

        public PropertyModel(Guid id, string name, string category)
            : base(id)
        {
            Name = name;
            Category = category;
        }
    }

    public class OperatorModel : Model
    {
        public string Name { get; }
        public IArray<PropertyModel> Properties { get; }

        public OperatorModel(Guid id, string name, IArray<PropertyModel> properties)
            : base(id)
        {
            Name = name;
            Properties = properties;
        }
    }

    public class NodeModel : Model
    {
        public string Name => Operators?.FirstOrDefault()?.Name ?? string.Empty;

        public IArray<OperatorModel> Operators { get; }

        public NodeModel(Guid id, IArray<OperatorModel> operators)
            : base(id)
        {
            Operators = operators;
        }
    }

    public class ConnectorModel : Model
    {
        public ConnectorModel(Guid id, Guid srcId, Guid destId)
            : base(id)
        {
            SourceId = srcId;
            DestinationId = destId;
        }

        public Guid SourceId { get; }
        public Guid DestinationId { get; }
    }

    public class GraphModel : Model
    {
        public GraphModel(Guid id, IArray<NodeModel> nodes, IArray<ConnectorModel> connectors)
            : base(id)
        {
            Nodes = nodes;
            Connectors = connectors;
        }

        public IArray<NodeModel> Nodes { get; }
        public IArray<ConnectorModel> Connectors { get; }
    }
}