using System;

namespace Ara3D.Collections
{
    public readonly struct Set<T>
        : ISet<T>
    {
        public Set(Func<T, bool> predicate) => Predicate = predicate;
        public Func<T, bool> Predicate { get; }
        public bool Contains(T item) => Predicate(item);
    }

    /// <summary>
    /// A monotonically increasing sequence of integers 
    /// </summary>
    public interface IRange : IOrderedArray<int>, ISet<int>
    {
        int From { get; }
    }


}