namespace Peacock;

/// <summary>
/// A default implementation of IBehavior that does nothing, and is intended to be used
/// as a base class for other behaviors. 
/// </summary>
/// TODO: the "ControlId" is what again? Is it necessary? How is it used
public record Behavior<TState>(object? ControlId) 
    : IBehavior
    where TState : new()
{
    public TState State { get; init; } 
        = new();

    public virtual ICanvas PreDraw(ICanvas canvas, IControl control)
        => canvas;

    public virtual ICanvas PostDraw(ICanvas canvas, IControl control)
        => canvas;

    public virtual IUpdates Process(IControl control, InputEvent input, IUpdates updates)
        => updates;

    public IUpdates UpdateState(IUpdates updates, Func<TState, TState> update)
        => updates.UpdateBehavior(this, x => x.WithState(update(x.State)));

    public Behavior<TState> WithState(TState state)
        => this with { State = state };
}