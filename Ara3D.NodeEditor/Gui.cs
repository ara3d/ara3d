using Ara3D.Collections;

namespace Ara3D.NodeEditor;

public abstract class Gui
{
    /// <summary>
    /// A stateful class represet
    /// </summary>
    public class GuiState
    {
        public IArray<IBehaviorTrigger> Triggers { get; set; }
        public IControlTemplate RootControlTemplate { get; set; }
        public Control RootControl { get; set; }
        public UserInput UserInput { get; set; }
        public ICanvas Canvas { get; set; }
        public bool IsDirty { get; set; }
    }

    // Current 
    public GuiState State = new GuiState();

    // Functions for the app to customize the UI 
    public void SetBehaviorTriggers(IArray<IBehaviorTrigger> triggers) 
        => State.Triggers = triggers;

    // Initialize the system. 
    public void Initialize(ControlTemplate template)
    {
        State.RootControlTemplate = template;
        State.IsDirty = true;
    }

    public void UpdateRootModel(IModel model)
    {
        throw new NotFiniteNumberException();
        //State.RootControl = State.RootControl.UpdateFromModel(model);
        State.IsDirty = true;
    }

    // This is called by the host platform has to provide.
    
    // This is called by the host platform each frame 
    public ICanvas HostOnFrame(ICanvas canvas, UserInput input)
    {
        State.IsDirty = false;
        
        var newRoot = State.RootControl;
        newRoot = UpdateBehaviors(newRoot, newRoot, input);
        State.RootControl = newRoot;

        var root2 = ApplyBehavior(newRoot);
        root2 = RecomputeGeometry(root2);
        return Draw(root2, canvas);
    }

    // Implementation functions
     
    public static Control UpdateBehaviors(Control control, Control root, UserInput input)
    {
        var r = control with { Behaviors = control.Behaviors.Select(b => b.Update(input, root)).Where(b => b != null).ToIArray() };
        return r with { Children = r.Children.Select(c => UpdateBehaviors(c, root, input)) };
    }

    public static Control ApplyBehavior(Control control)
    {
        var r = control;
        foreach (var b in r.Behaviors.Enumerate())
            r = b.Apply(r);
        return r with { Children = r.Children.Select(ApplyBehavior) };
    }

    public static Control ApplyBehaviors(Control control)
    {
        foreach (var b in control.Behaviors.Enumerate())
            control = b.Apply(control);
        return control;
    }

    public static Control RecomputeGeometry(Control control)
    {
        throw new NotImplementedException();
    }

    public Control ProcessBehaviorTriggers(Control control, Control root, UserInput input, IArray<IBehaviorTrigger> triggers)
    {
        // TEMP:
        return control;
    }

    public static ICanvas Draw(Control control, ICanvas canvas)
    {
        foreach (var b in control.Behaviors.Enumerate())
            canvas = b.PreDraw(canvas);

        canvas = control.Template.PreDraw(canvas, control);
        foreach (var c in control.Children.Enumerate())
            canvas = Draw(c, canvas);
        canvas = control.Template.PostDraw(canvas, control);

        foreach (var b in control.Behaviors.Enumerate())
            canvas = b.PostDraw(canvas);

        return canvas;
    }
}