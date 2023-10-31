using System.Windows;

namespace Peacock;

/// <summary>
/// A model interface identifies data types as being used for the model,
/// as opposed to the view, or some other role. 
/// </summary>
public interface IModel 
{
    Guid Id { get; }
}

/// <summary>
/// This provides a mapping from models to controls.
/// Using a factory simplifies theming and making broad changes,
/// because different factories can be used, each with their own
/// settings and state. Factories are implemented by applications
/// and provided by the framework. 
/// </summary>
public interface IControlFactory
{
    IEnumerable<IControl> Create(IModel model, Rect rect);
}