using System;

namespace Ara3D.Buffers
{
    /// <summary>
    /// Provides an interface to an object that manages a potentially
    /// large array of elements all of the same unmanaged type.
    /// </summary>
    public interface IBuffer
    {
        Array Data { get; }
        void WithPointer(Action<IntPtr> action);
        int ElementSize { get; }
    }

    /// <summary>
    /// A version of the IBuffer interface when the element types are known.
    /// </summary>
    public interface IBuffer<out T> : IBuffer
        where T: unmanaged
    {
        T[] GetTypedData();
    }

    /// <summary>
    /// Represents a buffer associated with a string name. 
    /// </summary>
    public interface INamedBuffer : IBuffer
    {
        string Name { get; }
    }

    /// <summary>
    /// A version of the INamedBuffer interface when the element types are known
    /// </summary>
    public interface INamedBuffer<out T> 
        : INamedBuffer, IBuffer<T> where T: unmanaged
    {
    }

    /// <summary>
    /// A concrete implementation of IBuffer
    /// </summary>
    public unsafe class Buffer<T> : IBuffer<T> where T : unmanaged
    {
        public Buffer(T[] data) => _data = data;

        public void WithPointer(Action<IntPtr> action)
        {
            fixed (T* p = _data)
            {
                action((IntPtr)p);
            }
        }

        public int ElementSize => sizeof(T);
        public Array Data => _data;
        private readonly T[] _data;
        public T[] GetTypedData() => _data;
    }

    /// <summary>
    /// A concrete implementation of INamedBuffer
    /// </summary>
    public class NamedBuffer : INamedBuffer
    {
        public NamedBuffer(IBuffer buffer, string name) => (Buffer, Name) = (buffer, name);
        public IBuffer Buffer { get; }
        public string Name { get; }
        public void WithPointer(Action<IntPtr> action) => Buffer.WithPointer(action);
        public int ElementSize => Buffer.ElementSize;
        public Array Data => Buffer.Data;
    }

    /// <summary>
    /// A concrete implementation of INamedBuffer with a specific type.
    /// </summary>
    public class NamedBuffer<T> : Buffer<T>, INamedBuffer<T> where T : unmanaged
    {
        public NamedBuffer(T[] data, string name) : base(data) => Name = name;
        public string Name { get; }
    }

    public unsafe class ByteSpanBuffer : INamedBuffer<byte>
    {
        public readonly ByteSpan Span;

        public ByteSpanBuffer(ByteSpan span, string name)
        {
            Span = span;
            Name = name;
        }

        public byte[] GetTypedData()
            => Span.ToArray();

        public Array Data 
            => throw new NotImplementedException();

        public void WithPointer(Action<IntPtr> action)
            => action(new IntPtr(Span.Ptr));

        public int ElementSize => 1;

        public string Name { get; }

        public static ByteSpanBuffer Create(ByteSpan span, string name)
            => new ByteSpanBuffer(span, name);
    }
}
