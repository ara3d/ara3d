namespace Ara3D.NodeEditor;

// Many behaviors are also animations. 
// They contain their own state, independent of the base control. 
// This allows controls to be composable via behaviors. 
// They are also used to signal changes to the model. 
// This is quite unlike everything else that has come before. 
// The idea is that I want my UI to be able to be animated. 

public class BaseBehavior : IBehavior
{
    public virtual IBehavior Update(UserInput input, Control controlRoot)
        => this;

    public virtual Control Apply(Control control)
        => control;

    public virtual IModel ToModel(Control control)
        => control.View.Model;

    public virtual ICanvas PreDraw(ICanvas canvas)
        => canvas;

    public ICanvas PostDraw(ICanvas canvas)
        => canvas;
}

public class NodeResizeBehavior : BaseBehavior
{ }

public class NodeHoverBehavior : BaseBehavior
{ }

public class NodePulsingBehavior : BaseBehavior
{ }

public class TooltipBehavior : BaseBehavior
{ }

public class SelectBehavior : BaseBehavior 
{ }

public class ConnectingBehavior : BaseBehavior
{ }

public class SnapableBehavior : BaseBehavior
{ }

public class SnappedBehavior : BaseBehavior
{ }

public class ConnectableBehavior : BaseBehavior
{ }

public class UndoBehavior : BaseBehavior
{ }

public class UnsnapBehavior : BaseBehavior
{ }

public class ClickBehavior : BaseBehavior 
{ }

public class ExpandNodeBehavior : BaseBehavior
{ }

public class CollapseNodeBehavior : BaseBehavior
{ }

public class DeleteBehavior : BaseBehavior
{ }

public class CreateBehavior : BaseBehavior
{ }
