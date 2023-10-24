using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Ara3D.Domo.Tests
{

    public interface IObservableList<T> : IList<T>, INotifyCollectionChanged { }

    public class ObservableList<T> : IList<T>, INotifyCollectionChanged
    {
        private ObservableCollection<T> _collection;

        public T this[int index] { get => ((IList<T>)_collection)[index]; set => ((IList<T>)_collection)[index] = value; }

        public int Count => ((ICollection<T>)_collection).Count;

        public bool IsReadOnly => ((ICollection<T>)_collection).IsReadOnly;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => ((INotifyCollectionChanged)_collection).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)_collection).CollectionChanged -= value;
        }

        public void Add(T item)
            => ((ICollection<T>)_collection).Add(item);

        public void Clear()
            => ((ICollection<T>)_collection).Clear();

        public bool Contains(T item)
            => ((ICollection<T>)_collection).Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => ((ICollection<T>)_collection).CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)_collection).GetEnumerator();

        public int IndexOf(T item)
            => ((IList<T>)_collection).IndexOf(item);

        public void Insert(int index, T item)
            => ((IList<T>)_collection).Insert(index, item);

        public bool Remove(T item)
            => ((ICollection<T>)_collection).Remove(item);

        public void RemoveAt(int index)
            => ((IList<T>)_collection).RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_collection).GetEnumerator();
    }
}