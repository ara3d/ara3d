using System.Data;
using System.Windows;
using System.Windows.Input;
using Ara3D.Collections;

namespace Ara3D.NodeEditor
{
    public record Geometry(
        Rect BoundingRect,
        Point Position,
        Size DesiredSize,
        IArray<Point> Points = null)
    {
        public Geometry(Rect rect)
            : this(rect, rect.TopLeft, rect.Size)
        { }

        public Geometry UpdateBounds(Rect rect)
            => this with  { BoundingRect = rect };
    }

    public record Control(
        IControlTemplate Template,
        View View,
        IArray<Control> Children,
        IArray<IBehavior> Behaviors)
    {
        public Control UpdateBounds(Rect rect)
            => this with { View = View.UpdateBounds(rect) };

        public Rect Bounds 
            => View.Geometry.BoundingRect;

        public IModel Model
            => View.Model;

        public static Control Create(IControlTemplate template, View view)
            => new(template, view, LinqArray.Empty<Control>(), LinqArray.Empty<IBehavior>());
    }

    public record View(IModel Model, string Text, Style Style, Geometry Geometry, object State)
    {
        public View UpdateBounds(Rect rect)
            => this with { Geometry = Geometry.UpdateBounds(rect) };
    }

    /// <summary>
    /// A set of style options associated with a kind of control. 
    /// </summary>
    public class StyleOptions
    {
        public Dictionary<string, Style> Styles = new();
        public Style this[string name] => Styles.TryGetValue(name, out var result) ? result : Style.Empty;
        public Style Default => this["default"];
    }

    /// <summary>
    /// A dictionary of values. 
    /// </summary>
    public class Style
    {
        public static readonly Style Empty = new Style();
        public Dictionary<string, object> Properties = new();
    }

   
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
        IArray<IBehavior> GetNewBehaviors(UserInput input, Control current, Control root);
    }

    /// <summary>
    /// Represents data stored outside the UI framework. 
    /// </summary>
    public interface IModel
    {
        Guid Id { get; }
    }

    /// <summary>
    /// Manages the construction, functionality, behavior, and presentation of a control.
    /// Controls themselves are just collections of state. 
    /// </summary>
    public interface IControlTemplate
    {
        Control Create(IModel model, Control parent);
        StyleOptions StyleOptions { get; }
        Rect ComputeBounds(Control child);
        IArray<Control> CreateChildren(Control parent);
        ICanvas PreDraw(ICanvas canvas, Control control);
        ICanvas PostDraw(ICanvas canvas, Control control);
    }
    

/*
= Update logic:

== On User Input 

- ProcessInput
    - For each behavior
    - Update if needed behaviors (some might get deleted)

- Look at triggers 
    - Add new behaviors if necessary 

- Iterate over all behaviors 
    - Get an updated view based on the behavior (need to keep the old one)
        - Where is it stored?
    - The behavior is applied to the original 

== On Redraw 

- Draw control 
    - PreDraw behaviors
    - Get new view by applying behaviors 
    - PreDraw control
        - Draw children
    - PostDraw control 
    - PostDraw behaviors 

== On Model Changed  

- Model added
    - Create new control 
    - Walk tree: put it where it should go 
    - Leave everything else as-is 
        - except, this suggests that the geometry might need to be recomputed of the containing control 
            - this is recursive
   
- Model deleted
    - Walk tree: remove the associated control 

- Model changed
    - Update control 
        - Create children control
        - Restore the old View state (text, geometry)
        - Note: the old 
   - Compute geometry
        (may compute)

    //==

    */
}