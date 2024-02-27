using System.Collections.Generic;

namespace Ara3D.Utils
{
    public class MultiDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, new List<TValue>());
            this[key].Add(value);
        }
    }
}