namespace Ara3D.NodeEditor;

/// <summary>
/// This coordinates an entire GUI canvas.
/// It coordinates the communication of a controller and the underlying GUI framework. 
/// There is one controller, a root control, and a set of behaviors.
/// The controller is responsible for creating the model, view, and control objects.
/// </summary>
public class Gui
{
    public Gui(IController controller)
    {
        Controller = controller;
    }

    IController Controller { get; }
    Control RootControl { get; set; }
    List<IBehaviorTrigger> Triggers { get; } 
      
    // Functions for the app to customize the UI 
    public void AddBehaviorTriggers(params IBehaviorTrigger[] triggers) 
        => Triggers.AddRange(triggers); 

    public IModel GetUpdatedModel()
        => Controller.CreateModel(RootControl);
    
    public void SetNewModel(IModel model)
    {
        RootControl = Controller.CreateControl(RootControl, model);
    }

    // This is called by the host platform has to provide.
    public ICanvas OnFrameUpdate(ICanvas canvas, UserInput input)
    {
        var newRoot = RootControl;
        newRoot = UpdateBehaviors(newRoot, input);
        RootControl = newRoot;

        var root2 = ApplyBehaviors(newRoot);
        return DrawControl(root2, canvas);
    }

    /// <summary>
    /// Creates a new version of the control with new versions of the behaviors 
    /// </summary>
    public static Control UpdateBehaviors(Control control, UserInput input)
    {
        var r = control with { Behaviors = control.Behaviors.Select(b => b.Update(input, control)).Where(b => b != null).ToList() };
        // TODO: iterate through all behavior triggers. See which one created a new behavior, concatenate it to the list of bh
        return r with { Children = r.Children.Select(c => UpdateBehaviors(c, input)).ToList() };
    }

    /// <summary>
    /// Creates a temporary version of the control tree with changes applied to the view as a result of the behaviors
    /// </summary>
    public static Control ApplyBehaviors(Control control)
    {
        var children = control.Children.Select(ApplyBehaviors).ToList();
        var r = control with { Children = children };
        return r.Behaviors.Aggregate(r, (current, b) => b.Apply(current));
    }

    /// <summary>
    /// Predraws the behaviors, then the view, then postdraws the behaviors.  
    /// </summary>
    public static ICanvas DrawControl(Control control, ICanvas canvas)
    {
        foreach (var b in control.Behaviors)
            canvas = b.PreDraw(canvas, control);

        canvas = control.View.Draw(canvas);
        foreach (var c in control.Children)
            canvas = DrawControl(c, canvas);

        foreach (var b in control.Behaviors)
            canvas = b.PostDraw(canvas, control);

        return canvas;
    }

        
}