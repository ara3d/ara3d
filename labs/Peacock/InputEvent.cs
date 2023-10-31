using System.Windows;
using System.Windows.Input;

namespace Peacock;

// This identifies type that contain input event data
// This could be generated from the Window system or from parent component 
public interface IInputEvent
{
}

// Keyboard, mouse, touch, file system, other 

public interface IMouseStatus
{
    Point Location { get; }
    bool LButtonDown { get; }
    bool RButtonDown { get; }
    bool MButtonDown { get; }
}

public record InputEvent : IInputEvent
{
    public IMouseStatus MouseStatus { get; set; }
}

public record KeyDownEvent(KeyEventArgs Args) : InputEvent;
public record KeyUpEvent(KeyEventArgs Args) : InputEvent;
public record MouseDoubleClickEvent(MouseButtonEventArgs Args) : InputEvent;
public record MouseDownEvent(MouseButtonEventArgs Args) : InputEvent;
public record MouseUpEvent(MouseButtonEventArgs Args) : InputEvent;
public record MouseMoveEvent(MouseEventArgs Args) : InputEvent;
public record MouseWheelEvent(MouseWheelEventArgs Args) : InputEvent;
public record ResizeEvent(SizeChangedEventArgs Args) : InputEvent;
public record ClockEvent(double ElapsedSeconds) : InputEvent;
public record PropsEvent(WindowProps Props) : InputEvent;
