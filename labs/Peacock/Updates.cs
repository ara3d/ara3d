namespace Peacock;

[Mutable]
public class Updates : IUpdates
{
    public Dictionary<object, List<Func<IControl, IControl>>> ControlUpdates { get; } = new();
    public Dictionary<IBehavior, List<Func<IBehavior, IBehavior>>> BehaviorUpdates { get; } = new();
    public Dictionary<Guid, List<Func<IModel, IModel>>> ModelUpdates { get; } = new();
    public Dictionary<object, List<IBehavior>> NewBehaviors { get; } = new();

    public IUpdates StoreUpdate<T, TKey>(TKey key, Func<T, T> updateFunc, Dictionary<TKey, List<Func<T, T>>> lookup)
    {
        if (!lookup.ContainsKey(key))
            lookup.Add(key, new());
        lookup[key].Add(updateFunc);
        return this;
    }

    public IUpdates AddBehavior(IControl control, IBehavior behavior)
    {
        if (!NewBehaviors.ContainsKey(control))
            NewBehaviors.Add(control, new());
        NewBehaviors[control].Add(behavior);
        return this;
    }

    public IUpdates UpdateControl(IControl key, Func<IControl, IControl> updateFunc)
        => StoreUpdate(key.View.Id, updateFunc, ControlUpdates);

    public IUpdates UpdateBehavior(IBehavior key, Func<IBehavior, IBehavior> updateFunc)
        => StoreUpdate(key, updateFunc, BehaviorUpdates);

    public IUpdates UpdateModel(IModel key, Func<IModel, IModel> updateFunc)
        => StoreUpdate(key.Id, updateFunc, ModelUpdates);

    public IControl Apply(IControl control)
        => ControlUpdates.ContainsKey(control.View.Id)
            ? ControlUpdates[control.View.Id].Aggregate(control, (local, func) => func(local))
            : control;

    public IBehavior Apply(IBehavior behavior)
        => BehaviorUpdates.ContainsKey(behavior)
            ? BehaviorUpdates[behavior].Aggregate(behavior, (local, func) => func(local))
            : behavior;

    public IModel Apply(IModel model)
        => ModelUpdates.ContainsKey(model.Id)
            ? ModelUpdates[model.Id].Aggregate(model, (local, func) => func(local))
            : model;
}
