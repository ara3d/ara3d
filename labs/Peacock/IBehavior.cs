namespace Peacock;

/// <summary>
/// A behavior is associated with a control and provide additional capabilities,
/// not defined in the control itself. You can think of it as a generalization of
/// event handling. Rather than connecting to one event at a time,
/// a behavior can handle any number of special types and manage its own state. 
/// Behaviors also can be drawn. 
/// A behavior can process input and update itself in response to user input.
/// Some example use cases of behaviors:
/// * Drawing a border
/// * Animation effects on specific event MouseMove/Enter/Leve/Down
/// * Changing cursor in response to events 
/// * Tooltips when mouse is hovered
/// * Managing and representing enabled/disabled state. 
/// * Making a control draggable
/// </summary>
public interface IBehavior
{
    object? ControlId { get; }
    ICanvas PreDraw(ICanvas canvas, IControl control);
    ICanvas PostDraw(ICanvas canvas, IControl control);
    IUpdates Process(IControl control, InputEvent input, IUpdates updates);
}