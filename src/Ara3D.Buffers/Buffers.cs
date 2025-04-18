using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Ara3D.Buffers
{
    /// <summary>
    /// Provides an interface to an object that manages an array of unmanaged memory.
    /// </summary>
    public interface IBuffer
    {
        int ElementSize { get; }
        Type ElementType { get; }
        int ElementCount { get; }
        object this[int i] { get; set; }
        Span<T> Span<T>() where T : unmanaged;
    }

    /// <summary>
    /// Represents a buffer associated with a string name. 
    /// </summary>
    public interface IBuffer<T> : IBuffer, IReadOnlyList<T> 
    {
        new T this[int i] { get; set; }
        Span<T> Span();
    }

    /// <summary>
    /// Represents a buffer associated with a string name. 
    /// </summary>
    public interface INamedBuffer : IBuffer
    {
        string Name { get; }
    }

    /// <summary>
    /// Represents a buffer associated with a string name. 
    /// </summary>
    public interface INamedBuffer<T> : IBuffer<T>, INamedBuffer
    {
    }

    /// <summary>
    /// A concrete implementation of IBuffer
    /// </summary>
    public unsafe class Buffer<T> : IBuffer<T>
        where T : unmanaged
    {
        public Buffer(T[] data) => _data = data;
        public int ElementSize => sizeof(T);
        private readonly T[] _data;
        public int ElementCount => _data.Length;
        public int Count => _data.Length;

        public T this[int i]
        {
            get => _data[i];
            set => _data[i] = value;
        }

        public Span<T> Span()
            => Span<T>();

        public Span<T0> Span<T0>() where T0 : unmanaged
            => MemoryMarshal.Cast<T, T0>(_data);

        public Type ElementType => typeof(T);
        
        object IBuffer.this[int i]
        {
            get => this[i];
            set => this[i] = (T)value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i= 0; i < ElementCount; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public class SlicedBuffer<T> : IBuffer<T>
        where T : unmanaged
    {
        public IBuffer<T> Original;
        public int Offset;
        public int ElementCount { get; }
        public int Count => ElementCount;

        public int ElementSize => Original.ElementSize;
        public SlicedBuffer(IBuffer<T> original, int offset, int count)
        {
            Original = original;
            Offset = offset;
            Debug.Assert(count >= 0);
            Debug.Assert(count <= original.Count);
            ElementCount = count;
        }

        public T this[int i]
        {
            get => Original[i + Offset];
            set => Original[i + Offset] = value;
        }

        public Span<T0> Span<T0>() where T0 : unmanaged
            => Original.Span<T0>().Slice(Offset, ElementCount);

        public Span<T> Span()
            => Span<T>();

        public Type ElementType => typeof(T);

        object IBuffer.this[int i]
        {
            get => this[i];
            set => this[i] = (T)value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i=0; i < ElementCount; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public unsafe class CastBuffer<T> : IBuffer<T>
        where T : unmanaged
    {
        public IBuffer Original;
        public int ElementSize => sizeof(T);
        public int ElementCount { get; }
        public int Count => ElementCount;

        public CastBuffer(IBuffer original)
        {
            Original = original;
            
            ElementCount = (int)(original.GetNumBytes() / ElementSize);

            if (this.GetNumBytes() != original.GetNumBytes())
                throw new Exception("The original buffer cannot be cast into the new buffer");
        }

        public Span<T0> Span<T0>() where T0 : unmanaged
            => Original.Span<T0>();

        public T this[int i]
        {
            get => Span()[i];
            set => Span()[i] = value;
        }

        public Span<T> Span()
            => Span<T>();

        public Type ElementType => typeof(T);

        object IBuffer.this[int i]
        {
            get => this[i];
            set => this[i] = (T)value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < ElementCount; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    /// <summary>
    /// A concrete implementation of INamedBuffer
    /// </summary>
    public class NamedBuffer : INamedBuffer
    {
        public NamedBuffer(IBuffer buffer, string name) => (Buffer, Name) = (buffer, name);
        public IBuffer Buffer { get; }
        public string Name { get; }
        public int ElementSize => Buffer.ElementSize;
        public int ElementCount => Buffer.ElementCount;
        public Span<T> Span<T>() where T : unmanaged => Buffer.Span<T>();

        public Type ElementType => Buffer.ElementType;

        object IBuffer.this[int i]
        {
            get => Buffer[i];
            set => Buffer[i] = value;
        }
    }

    /// <summary>
    /// A concrete implementation of INamedBuffer with a specific type.
    /// </summary>
    public class NamedBuffer<T> : Buffer<T>, INamedBuffer<T> where T : unmanaged
    {
        public NamedBuffer(IBuffer<T> data, string name) : base(data.ToArray()) => Name = name;
        public NamedBuffer(T[] data, string name) : base(data) => Name = name;
        public string Name { get; }
    }

    // TODO: this needs to be removed, along with ByteSpan 
    public unsafe class ByteSpanBuffer 
    {
        public readonly ByteSpan ByteSpan;

        public ByteSpanBuffer(ByteSpan byteSpan, string name)
        {
            ByteSpan = byteSpan;
            Name = name;
        }

        public void WithPointer(Action<IntPtr> action)
            => action(new IntPtr(ByteSpan.Ptr));

        public int ElementSize => 1;
        public int Count => ByteSpan.Length;
        public Span<T> Span<T>() where T : unmanaged
            => MemoryMarshal.Cast<byte, T>(ByteSpan.ToSpan());

        public string Name { get; }

        public static ByteSpanBuffer Create(ByteSpan span, string name)
            => new ByteSpanBuffer(span, name);
    }
}
