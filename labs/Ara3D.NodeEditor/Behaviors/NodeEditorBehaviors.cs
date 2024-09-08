using System.Windows;

namespace Ara3D.NodeEditor;

// Many behaviors are also animations. 
// They contain their own state, independent of the base control. 
// This allows controls to be composable via behaviors. 
// They are also used to signal changes to the model. 
// This is quite unlike everything else that has come before. 
// The idea is that I want my UI to be able to be animated. 

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

public class KeyDownBehavior : BaseBehavior
{ }

public class MouseOverBehavior : BaseBehavior
{ }

public class BehaviorTrigger<TBehavior> : IBehaviorTrigger where TBehavior : IBehavior, new()
{
    public readonly TriggerCondition Condition;

    public BehaviorTrigger(TriggerCondition condition)
        => Condition = condition;

    public IBehavior? Triggered(UserInput input, Control control)
        => Condition.Met(input, control) ? new TBehavior() : null;
}

public class TriggerCondition
{
    public readonly Func<UserInput, Control, bool> Predicate;

    public TriggerCondition(Func<UserInput, Control, bool> predicate)
        => Predicate = predicate;

    public bool Met(UserInput input, Control control)
        => Predicate(input, control);
}

public static class BehaviorTriggers
{
    public static IBehaviorTrigger Trigger<TBehavior>(this TriggerCondition condition)
        where TBehavior : IBehavior, new()
        => new BehaviorTrigger<TBehavior>(condition);

    public static TriggerCondition Trigger()
        => new((_, _) => true);

    public static TriggerCondition IsModelType<TModel>(this Type modelType)
        => new((_, control) => control.Model.GetType() == typeof(TModel));

    public static TriggerCondition And(this TriggerCondition triggerA, Func<UserInput, Control, bool> predicate)
        => triggerA.And(new TriggerCondition(predicate));

    public static TriggerCondition And(this TriggerCondition triggerA, params TriggerCondition[] triggers)
        => new((input, control) => triggerA.Met(input, control) && triggers.All(t => t.Met(input, control)));

    public static TriggerCondition IsMouseOver(this TriggerCondition trigger)
        => trigger.And((input, control) => control.View.HitTest(input.Mouse));

    public static TriggerCondition HasBehavior<TBehavior>(this TriggerCondition condition)
        => condition.And((input, control) => control.Behaviors.Any(b => b is TBehavior));   

    public static TriggerCondition Not(this TriggerCondition condition)
        => new((input, control) => !condition.Met(input, control));

    /*
    public static Trigger<MouseEnterBehavior>(this TriggerCondition condition)
        => condition.Hovering().And(condition.HasBehavior<MouseOverBehavior>().Not());
    */
}

public class MouseBehavior : BaseBehavior
{
    public readonly Point MousePoint;
    public readonly float Time;
}

public class MouseOver : MouseBehavior { }
public class MouseEnter: MouseBehavior { }
public class MouseLeave : MouseBehavior { }
public class MouseLButtonDown : MouseBehavior { }
public class MouseLButtonUp : MouseBehavior { }
public class MouseRButtonDown : MouseBehavior { }
public class MouseRButtonUp : MouseBehavior { }
public class MouseButtonDoubleClick : MouseBehavior { }
public class MouseDraggable : MouseBehavior { }
public class MouseDragStart : MouseBehavior { }
public class MouseDragEnd : MouseBehavior { }
public class MouseDragCancel : MouseBehavior { }
public class MouseDragMove : MouseBehavior { }

public static class CommonBehaviors
{

}