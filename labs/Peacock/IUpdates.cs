namespace Peacock;

/// <summary>
/// Represents a collection of proposed changes to the UI or application state.
/// An implementation of this class is supplied by the Peacock library.
/// 
/// These changes are aggregated and applied to the UI state by the control manager,
/// and then to the model by the application.
/// 
/// The IUpdates serves a similar role as the Reducer in a React application that uses
/// the Redux state management library. 
///
/// The ControlManager class is responsible for applying changes from an IUpdates to the internal
/// controls and behaviors, and triggering any IControl.Callback calls as well.
/// When controls are changed, they will be given an opportunity to apply model changes
/// to the IUpdates.
///
/// An application is responsible for looking at the updates and iterating over
/// all proposed changes to the model and applying them. 
/// </summary>
public interface IUpdates
{
    IUpdates UpdateBehavior(IBehavior key, Func<IBehavior, IBehavior> updateFunc);
    IUpdates UpdateControl(IControl key, Func<IControl, IControl> updateFunc);
    IUpdates UpdateModel(IModel key, Func<IModel, IModel> updateFunc);

    IBehavior Apply(IBehavior behavior);
    IControl Apply(IControl control);
    IModel Apply(IModel model);
}

public static class UpdatesExtensions
{
    public static T ApplyToModel<T>(this IUpdates updates, T model) where T : IModel
        => (T)updates.Apply(model);

    public static T ApplyToControl<T>(this IUpdates updates, T control) where T : IControl
        => (T)updates.Apply(control);

    public static T ApplyToBehavior<T>(this IUpdates updates, T behavior) where T : IBehavior
        => (T)updates.Apply(behavior);

    public static IUpdates UpdateBehavior<TBehavior>(this IUpdates updates, TBehavior key, Func<TBehavior, TBehavior> updateFunc)
        where TBehavior : IBehavior
        => updates.UpdateBehavior(key, b => updateFunc((TBehavior)b));

    public static IUpdates UpdateControl<TControl>(this IUpdates updates, TControl key, Func<TControl, TControl> updateFunc)
        where TControl : IControl
        => updates.UpdateControl(key, c => updateFunc((TControl)c));

    public static IUpdates UpdateModel<TModel>(this IUpdates updates, TModel key, Func<TModel, TModel> updateFunc)
        where TModel : IModel
        => updates.UpdateModel(key, m => updateFunc((TModel)m));

}