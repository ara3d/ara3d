namespace Ara3D.NodeEditor;

public class Gui
{
    /// <summary>
    /// A stateful class representing the GUI 
    /// </summary>
    public class GuiState
    {
        public IReadOnlyList<IBehaviorTrigger> Triggers { get; set; }
        public ICoordinator Coordinator { get; set; }
        public Control RootControl { get; set; }
        public UserInput UserInput { get; set; }
        public ICanvas Canvas { get; set; }
        public bool IsDirty { get; set; }
    }

    // Current 
    public GuiState State = new GuiState();

    // Functions for the app to customize the UI 
    public void SetBehaviorTriggers(IReadOnlyList<IBehaviorTrigger> triggers) 
        => State.Triggers = triggers;

    // Initialize the system. 
    public void Initialize(Coordinator coordinator)
    {
        State.Coordinator = coordinator;
        State.IsDirty = true;
    }

    public void UpdateRootModel(IModel model)
    {
        throw new NotFiniteNumberException();
        //State.RootControl = State.RootControl.UpdateFromModel(model);
        State.IsDirty = true;
    }

    // This is called by the host platform has to provide.
    public ICanvas OnFrameUpdate(ICanvas canvas, UserInput input)
    {
        State.IsDirty = false;
        
        var newRoot = State.RootControl;
        newRoot = UpdateBehaviors(newRoot, newRoot, input);
        State.RootControl = newRoot;

        var root2 = ApplyBehavior(newRoot);
        return Draw(root2, canvas);
    }

    // Implementation functions
     
    public static Control UpdateBehaviors(Control control, Control root, UserInput input)
    {
        var r = control with { Behaviors = control.Behaviors.Select(b => b.Update(input, root)).Where(b => b != null).ToList() };
        return r with { Children = r.Children.Select(c => UpdateBehaviors(c, root, input)).ToList() };
    }

    public static Control ApplyBehavior(Control control)
    {
        var r = control;
        foreach (var b in r.Behaviors)
            r = b.Apply(r);
        return r with { Children = r.Children.Select(ApplyBehavior).ToList() };
    }

    public static Control ApplyBehaviors(Control control)
    {
        foreach (var b in control.Behaviors)
            control = b.Apply(control);
        return control;
    }

    public Control ProcessBehaviorTriggers(Control control, Control root, UserInput input, IReadOnlyList<IBehaviorTrigger> triggers)
    {
        // TEMP:
        return control;
    }

    public static ICanvas Draw(Control control, ICanvas canvas)
    {
        foreach (var b in control.Behaviors)
            canvas = b.PreDraw(canvas);

        canvas = control.View.Draw(canvas);
        foreach (var c in control.Children)
            canvas = Draw(c, canvas);

        foreach (var b in control.Behaviors)
            canvas = b.PostDraw(canvas);

        return canvas;
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