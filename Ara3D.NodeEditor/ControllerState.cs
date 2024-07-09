namespace Ara3D.NodeEditor;

public class ControllerState
{
    public Dictionary<Guid, IModel> Models { get; } = new Dictionary<Guid, IModel>();
    public Dictionary<Guid, IView> Views { get; } = new Dictionary<Guid, IView>();
    public Dictionary<Guid, Control> Controls { get; } = new Dictionary<Guid, Control>();
}