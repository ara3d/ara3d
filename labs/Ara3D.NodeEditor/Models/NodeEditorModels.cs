using System.Windows;
using Ara3D.Domo;

namespace Ara3D.NodeEditor
{
    public record SocketModel(Guid Id, string Name, string Type, int Position) 
    {
        public IEnumerable<IModel> Children => Enumerable.Empty<IModel>();
    }

    public record PropertyModel(Guid Id, string Name, int Index, string Type, string Category, SocketModel Left, SocketModel Right) 
        : Model(Id)
    {
        public override IEnumerable<IModel> Children => new[] { Left, Right };
    }

    public record OperatorModel(Guid Id, string Name, int Index, IReadOnlyList<PropertyModel> Properties) 
        : Model(Id)
    {
        public override IEnumerable<IModel> Children => Properties;
    }

    public record NodeModel(Guid Id, string Name, Rect Rect, IReadOnlyList<OperatorModel> Operators) 
        : Model(Id)
    {
        public override IEnumerable<IModel> Children => Operators;
    }

    public record ConnectorModel(Guid Id, Guid SourceId, Guid DestinationId) 
        : Model(Id)
    {
        public override IEnumerable<IModel> Children => Enumerable.Empty<IModel>();
    }

    public record GraphModel(Guid Id, IReadOnlyList<NodeModel> Nodes, IReadOnlyList<ConnectorModel> Connectors, Rect Rect) 
        : Model(Id)
    {
        public override IEnumerable<IModel> Children => Nodes.Cast<IModel>().Concat(Connectors);
    }
}