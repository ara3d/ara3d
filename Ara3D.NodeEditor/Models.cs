using System.Windows;

namespace Ara3D.NodeEditor
{
    public record Model(Guid Id) : IModel;
    public record PropertyModel(Guid Id, string Name, int Index, string Type, string Category) : Model(Id);
    public record OperatorModel(Guid Id, string Name, int Index, IReadOnlyList<PropertyModel> Properties) : Model(Id);
    public record NodeModel(Guid Id, string Name, Rect Rect, IReadOnlyList<OperatorModel> Operators) : Model(Id);
    public record ConnectorModel(Guid Id, Guid SourceId, Guid DestinationId) : Model(Id);
    public record GraphModel(Guid Id, IReadOnlyList<NodeModel> Nodes, IReadOnlyList<ConnectorModel> Connectors, Rect Rect) : Model(Id);
}