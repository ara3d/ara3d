using System.Windows;
using Ara3D.Domo;

namespace Ara3D.NodeEditor
{
    /// <summary>
    /// A class that contains parameters used to control how a view is drawn.
    /// Different views have different style. 
    /// </summary>
    public interface IStyle
    { }

    /// <summary>
    /// A behavior changes over time, in response to user input, the passing of time, or other behaviors. 
    /// A behavior can draw on the canvas before and after the view is drawn.
    /// A behavior can temporarily modify a view before it displays itself (e.g., change style, or dimensions)
    /// </summary>
    public interface IBehavior
    {
        IBehavior Update(UserInput input, Control control);
        Control Apply(Control control);
        ICanvas PreDraw(ICanvas canvas, Control control);
        ICanvas PostDraw(ICanvas canvas, Control control);
    }

    /// <summary>
    /// Based on user input, or the state of the control tree, a new behavior 
    /// can be triggered.  
    /// </summary>
    public interface IBehaviorTriggers
    {
        IReadOnlyList<IBehavior> GetNewBehaviors(UserInput input, Control current);
    }

    /// <summary>
    /// Manages updating controls from models and vice versa.
    /// Also responsible for creating behaviors. 
    /// </summary>
    public interface IController
    {
        IView CreateView(IModel model, IView? previousView = null);
        Control CreateControl(Control previousControl, IModel model);
        IModel CreateModel(Control control);
        IReadOnlyList<IBehavior> NewBehaviors(UserInput input, Control control);
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
    /// The view represents the state of a control.
    /// A view hold a generic point to a model, but doesn't know anything about the specifics. 
    /// A coordinator 
    /// </summary>
    public interface IView
    {
        IModel Model { get; }
        bool HitTest(Point point);
        ICanvas Draw(ICanvas canvas);
        Rect Rect { get; }
        IStyle Style { get; }
    }

    /// <summary>
    /// Returns a new behavior if a new one is created, or null otherwise. 
    /// </summary>
    public interface IBehaviorTrigger 
    {
         IBehavior? Triggered(UserInput input, Control control);
    }
}