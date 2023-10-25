using System;

namespace Ara3D.Collections
{
    // TODO: add support for indexing. 
    public class IndexableStack<T> : IStack<T>
    {
        public Buffer Current { get; set; } = new Buffer();

        public class Buffer
        {
            public T[] Values { get; }
            public int Capacity => Values.Length;
            public int Count { get; set; }

            public T this[int index]
            {
                get => Values[index];
                set => Values[index] = value;
            }

            public Buffer Next { get; set; }
            public bool IsFull => Count == Capacity;
            public bool IsEmpty => Capacity == 0;

            public Buffer(Buffer next = null)
            {
                if (next != null)
                    Values = new T[next.Capacity * 2];
                else
                    Values = new T[16];
                Next = next;
            }

            public void Add(T value)
                => Values[Count++] = value;

            public T Remove()
                => Values[--Count];
        }

        public IStack<T> Push(T x)
        {
            /*
            if (Current == null || Current.IsFull)
                Current = new Buffer(Current);
            Current.Add(x);
            */
            throw new NotImplementedException();
        }

        public IStack<T> Pop()
        {
            /*
            if (Current.IsEmpty)
                Current = Current.Next;
            return Current.Remove();
            */
            throw new NotImplementedException();
        }

        public T Peek()
        {
            /*
            var tmp = Pop();
            Push(tmp);
            return tmp;
            */
            throw new NotImplementedException();
        }

        public bool IsEmpty
            => Current == null || (Current.IsEmpty && Current.Next == null);
    }
}