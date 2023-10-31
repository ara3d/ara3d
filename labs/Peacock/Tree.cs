namespace Peacock;

public class Tree<T>
{
    public T Value { get; }
    public IReadOnlyList<Tree<T>> Children { get; }

    public Tree(T x, Func<T, IEnumerable<Tree<T>>> childrenGenerator)
        => (Value, Children) = (x, childrenGenerator(x).ToList());

    public IEnumerable<Tree<T>> AllNodes()
    {
        yield return this;
        foreach (var c in Children)
        foreach (var n in c.AllNodes())
            yield return n;
    }
}