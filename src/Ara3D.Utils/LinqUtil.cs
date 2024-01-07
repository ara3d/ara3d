using System;
using System.Collections.Generic;
using System.Linq;

namespace Ara3D.Utils
{
    public static class LinqUtil
    {
        // https://stackoverflow.com/questions/4823467/using-linq-to-find-the-cumulative-sum-of-an-array-of-numbers-in-c-sharp/

        public static IEnumerable<U> Accumulate<T, U>(this IEnumerable<T> self, U init, Func<U, T, U> f)
        {
            foreach (var x in self)
                yield return init = f(init, x);
        }

        public static IEnumerable<T> Accumulate<T>(this IEnumerable<T> self, Func<T, T, T> f)
            => self.Accumulate(default, f);

        public static IEnumerable<double> PartialSums(this IEnumerable<double> self)
            => self.Accumulate((x, y) => x + y);

        public static IEnumerable<int> PartialSums(this IEnumerable<int> self)
            => self.Accumulate((x, y) => x + y);

        public static IEnumerable<T> DifferentFromPrevious<T>(this IEnumerable<T> self)
        {
            var first = true;
            var prev = default(T);
            foreach (var x in self)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (!x.Equals(prev))
                        yield return x;
                }

                prev = x;
            }
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self)
            => self.Where(x => x != null);


        public static T Minimize<T, U>(this IEnumerable<T> xs, U init, Func<T, U> func) where U : IComparable<U>
        {
            var r = default(T);
            foreach (var x in xs)
            {
                if (func(x).CompareTo(init) < 0)
                {
                    init = func(x);
                    r = x;
                }
            }

            return r;
        }

        public static T Maximize<T, U>(this IEnumerable<T> xs, U init, Func<T, U> func) where U : IComparable<U>
        {
            var r = default(T);
            foreach (var x in xs)
            {
                if (func(x).CompareTo(init) > 0)
                {
                    init = func(x);
                    r = x;
                }
            }

            return r;
        }

        /// <summary>
        /// Returns the top of a stack, or the default T value if none is present.
        /// </summary>
        public static T PeekOrDefault<T>(this Stack<T> self)
            => self.Count > 0 ? self.Peek() : default;

        /// <summary>
        /// A substitute for SingleOrDefault which does not throw an exception when the collection size is greater than 1.
        /// If the collection size is greater than 1, returns default.
        /// </summary>
        public static T OneOrDefault<T>(this IEnumerable<T> values)
        {
            var items = values?.OfType<T>().ToList();
            return items?.Count == 1 ? items[0] : default;
        }

        /// <summary>
        /// Generic depth first traversal. Improved answer over:
        /// https://stackoverflow.com/questions/5804844/implementing-depth-first-search-into-c-sharp-using-list-and-stack
        /// </summary>
        public static IEnumerable<T> DepthFirstTraversal<T>(T root, Func<T, IEnumerable<T>> childGen,
            HashSet<T> visited = null)
            => DepthFirstTraversal(Enumerable.Repeat(root, 1), childGen, visited);

        /// <summary>
        /// Generic depth first traversal. Improved answer over:
        /// https://stackoverflow.com/questions/5804844/implementing-depth-first-search-into-c-sharp-using-list-and-stack
        /// </summary>
        public static IEnumerable<T> DepthFirstTraversal<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> childGen,
            HashSet<T> visited = null)
        {
            var stk = new Stack<T>();
            foreach (var root in roots)
                stk.Push(root);
            visited = visited ?? new HashSet<T>();
            while (stk.Count > 0)
            {
                var current = stk.Pop();
                if (!visited.Add(current))
                    continue;
                yield return current;
                var children = childGen(current);
                if (children != null)
                {
                    foreach (var x in children)
                    {
                        if (!visited.Contains(x))
                            stk.Push(x);
                    }
                }
            }
        }

        /// <summary>
        /// Generic breadth first traversal.
        /// </summary>
        public static IEnumerable<T> BreadthFirstTraversal<T>(T root, Func<T, IEnumerable<T>> childGen,
            HashSet<T> visited = null)
            => BreadthFirstTraversal(new[] { root }, childGen, visited);

        /// <summary>
        /// Generic breadth first traversal.
        /// </summary>
        public static IEnumerable<T> BreadthFirstTraversal<T>(this IEnumerable<T> roots,
            Func<T, IEnumerable<T>> childGen, HashSet<T> visited = null)
        {
            var q = new Queue<T>();
            foreach (var root in roots)
                q.Enqueue(root);
            visited = visited ?? new HashSet<T>();
            while (q.Count > 0)
            {
                var current = q.Dequeue();
                if (!visited.Add(current))
                    continue;
                yield return current;
                var children = childGen(current);
                if (children != null)
                {
                    foreach (var x in children)
                    {
                        if (!visited.Contains(x))
                            q.Enqueue(x);
                    }
                }
            }
        }

        /// <summary>
        /// A helper function for append one or more items to an IEnumerable.
        /// </summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> xs, params T[] x)
            => xs.Concat(x);

        /// <summary>
        /// Counts groups of a given key
        /// </summary>
        public static Dictionary<T, int> CountGroups<T>(this IEnumerable<T> self)
            => self.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        /// <summary>
        /// Groups items using the key selector.
        /// TODO: add DictionaryOfLists (or its this a MultiDictionary_
        /// </summary>
        public static Dictionary<TKey, List<TValue>> ToDictionaryOfLists<T, TKey, TValue>
            (this IEnumerable<T> self, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var groups = self.GroupBy(keySelector);
            return groups.ToDictionary(g => g.Key, g => g.Select(valueSelector).ToList());
        }

        /// <summary>
        /// Groups items using the key selector.
        /// </summary>
        public static Dictionary<TKey, List<T>> ToDictionaryOfLists<T, TKey>
            (this IEnumerable<T> self, Func<T, TKey> keySelector)
            => self.ToDictionaryOfLists(keySelector, x => x);

        public static TB MinWhere<TA, TB>(this IEnumerable<TA> self,
            Func<TA, TB> selector, Func<TB, bool> filter, TB defaultValue)
        {
            var tmp = self.Select(selector).Where(filter).ToList();
            return tmp.Count == 0 ? defaultValue : tmp.Min();
        }

        public static TB MaxWhere<TA, TB>(this IEnumerable<TA> self,
            Func<TA, TB> selector, Func<TB, bool> filter, TB defaultValue)
        {
            var tmp = self.Select(selector).Where(filter).ToList();
            return tmp.Count == 0 ? defaultValue : tmp.Max();
        }

        public static IEnumerable<T> Distinct<T, U>(this IEnumerable<T> self, Func<T, U> func)
            => self.Distinct(CreateEqualityComparer<T>(x => func(x).GetHashCode(), (a, b) => func(a).Equals(func(b))));

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> self, Func<T, T, bool> comparison, Func<T, int> hashFunc)
            => self.Distinct(CreateEqualityComparer(hashFunc, comparison));

        public static IEqualityComparer<T> CreateEqualityComparer<T>(
            Func<T, int> getHashCode,
            Func<T, T, bool> equals)
        {
            if (getHashCode == null) throw new ArgumentNullException(nameof(getHashCode));
            if (equals == null) throw new ArgumentNullException(nameof(equals));
            return new Comparer<T>(getHashCode, equals);
        }
    }
}