using System.Data;
using System.Windows;
using System.Windows.Input;
using Ara3D.Collections;

namespace Ara3D.NodeEditor
{
    /// <summary>
    /// A behavior might be modified over time, or might 
    /// Some behaviors might be triggered by a change in state. 
    /// It can modify a view state.
    /// </summary>
    public interface IBehavior
    {
        IBehavior Update(UserInput input, Control controlRoot);
        Control Apply(Control control);
        ICanvas PreDraw(ICanvas canvas);
        ICanvas PostDraw(ICanvas canvas);
    }

    /// <summary>
    /// Based on user input, or the state of the control tree, a new behavior
    /// can be triggered.  
    /// </summary>
    public interface IBehaviorTrigger
    {
        IReadOnlyList<IBehavior> GetNewBehaviors(UserInput input, Control current, Control root);
    }

    /// <summary>
    /// Represents data stored outside the UI framework. 
    /// </summary>
    public interface IModel
    {
        Guid Id { get; }
    }

    /// <summary>
    /// Manages updating controls from models and vice versa. 
    /// </summary>
    public interface ICoordinator
    {
        Control UpdateControlFromModel(Control control, IModel model);
        IModel UpdateModelFromControl(Control control);
    }

    /// <summary>
    /// A drawing abstraction.
    /// This can be used to support different drawing platforms. 
    /// </summary>
    public interface ICanvas
    {
        ICanvas Draw(StyledText text);
        ICanvas Draw(StyledLine line);
        ICanvas Draw(StyledEllipse ellipse);
        ICanvas Draw(StyledRect rect);
        Size MeasureText(StyledText text);
        ICanvas SetRect(Rect rect);
        ICanvas PopRect();
    }

    /// <summary>
    /// The view contains the state of a control. 
    /// Models can update themselves based on a view.
    /// They don't know anything about the model.  
    /// </summary>
    public interface IView
    {
        IModel Model { get; }
        bool HitTest(Point point);
        ICanvas Draw(ICanvas canvas);
        IView Update(IView view);
    }
}