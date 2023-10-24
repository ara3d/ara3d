using System;
using System.Collections.Generic;

namespace TreeViewFileExplorer
{
    [Serializable]
    public abstract class BaseObject : PropertyNotifier
    {
        private IDictionary<string, object> _values 
            = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public T GetValue<T>(string key)
            => GetValue(key) is T x ? x : default;

        private object GetValue(string key) 
            => !string.IsNullOrEmpty(key) && _values.ContainsKey(key) ? _values[key] : null;

        public void SetValue(string key, object value)
        {
            if (!_values.ContainsKey(key))
            {
                _values.Add(key, value);
            }
            else
            {
                _values[key] = value;
            }
            OnPropertyChanged(key);
        }
    }
}
