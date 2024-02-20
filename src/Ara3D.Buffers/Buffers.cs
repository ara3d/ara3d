using System;
using System.IO;

namespace Ara3D.Buffers
{
    /// <summary>
    /// Provides an interface to an object that manages a potentially large array of elements all of the same unmanaged type.
    /// </summary>
    public interface IBuffer
    {
        Array Data { get; }
        void WithPointer(Action<IntPtr> action);
        int ElementSize { get; }
        // TODO: why would this be required as part of the interface?
        void Write(Stream stream);
    }

    /// <summary>
    /// A version of the IBuffer interface when the element types are known
    /// </summary>
    public interface IBuffer<out T> : IBuffer
    {
        // TODO: Shouldn't this be an extension method on IBuffer?
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
    public interface INamedBuffer<T> : INamedBuffer, IBuffer<T>
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
        public void Write(Stream stream) => stream.Write(GetTypedData());
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
        public void Write(Stream stream) => Buffer.Write(stream);
    }

    /// <summary>
    /// A concrete implementation of INamedBuffer with a specific type.
    /// </summary>
    public class NamedBuffer<T> : Buffer<T>, INamedBuffer<T> where T : unmanaged
    {
        public NamedBuffer(T[] data, string name) : base(data) => Name = name;
        public string Name { get; }
    }
}
