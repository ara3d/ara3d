using System.Collections.Generic;
using System.Linq;

namespace Ara3D.Utils
{
    /// <summary>
    /// Helper functions for working dictionaries. 
    /// </summary>
    public static class DictionaryUtil
    {
        public static IReadOnlyDictionary<TKey, TValue> Remove<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> self, TKey key)
            => self.Where(kv => !kv.Key.Equals(key)).ToDictionary(kv => kv.Key, kv => kv.Value);

        public static IReadOnlyDictionary<TKey, TValue> Add<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self,
            TKey key, TValue value)
            => self.Append(new KeyValuePair<TKey, TValue>(key, value)).ToDictionary(kv => kv.Key, kv => kv.Value);

        public static IReadOnlyDictionary<TKey, TValue> Update<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> self, TKey key, TValue value)
            => self.ToDictionary(kv => kv.Key, kv => kv.Key.Equals(key) ? value : kv.Value);

        public static IReadOnlyDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> self,
            TKey key, TValue value)
            => self.ContainsKey(key) ? self.Update(key, value) : self.Add(key, value);

        public static IReadOnlyDictionary<TKey, TValue> Clear<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> self)
            => new Dictionary<TKey, TValue>();
    }
}