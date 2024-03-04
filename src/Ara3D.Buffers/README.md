# Ara3D.Buffers

[![NuGet Version](https://img.shields.io/nuget/v/Ara3D.Buffers)](https://www.nuget.org/packages/Ara3D.Buffers)

Utilities for working with large arrays of unmanaged types.

## IBuffer

The `IBuffer` interface is presents a simple interface for an 
array of unmanaged types, where the type is not known at compile-time. 

This allows us to work more easily with collections of buffers where the 
underlying type can vary at run-time. 
 
 ```
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
```

The total number of bytes is determined by multiply the data array by the element size.
This is provided in an extension function

```
    public static long GetNumBytes(this IBuffer buffer)
        => (long)buffer.NumElements() * buffer.ElementSize;
```

Accessing the raw bytes can be done using a pattern similar to:

```
    public static unsafe void Write(this Stream stream, IBuffer buffer)
        => buffer.WithPointer(ptr => {
            stream.WriteBytesBuffered((byte*)ptr.ToPointer(), buffer.GetNumBytes());
        });
```

## INamedBuffer

Frequently a buffer is associated with a name or identifier. 
The `INamedBuffer` introduces a string name property, for convenience.   

## Use Case

This structure is particularly useful when serializing and manipulating raw memory such 
as attribute buffers associated with 3D geometry, ZIP files (or other binary archive formats), 
or color data associated with bitmaps. 

## Documentation 

This is a very small library, and we recommend reading the source code and looking at some projects 
that use it:

* Ara3D.Graphics
* Ara3D.Serialization.BFAST
* Ara3D.Serialization.G3D
* Ara3D.Serialization.VIM

## Size Limitations

Due to limitations in C# the array can have at most 2^31 
elements (approximately 2 billion). 


