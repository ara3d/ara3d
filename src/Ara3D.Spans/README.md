# Ara3D.Spans

This library contains a small utility class called `ByteSpan` which 
is an alternative to `ReadOnlySpan<byte>`. 

## What about Span 

The `Span` type is the .NET library is a useful could be used, but it is a `ref struct`.
The means it comes with several restrictions such as it can’t be stored in an array, or used as a generic argument.

For more information see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct. 

## About ByteSpan 

`Ara3D.Spans.ByteSpan` is designed specifically for working with pointers to unmanaged (e.g. pinned) memory. 

A `ByteSpan` consists of a literal pointer to a byte, and the number of bytes. 
It doesn’t have the `ref struct` limitations of `Span<byte>` data type and is faster 
for memory accessing (just grab the pointer and address it). 

The only caveat is you have to pin memory before using it, and be careful about avoiding accessing 
memory access overrun. 

Pinning memory prevents the garbage collector from moving it or freeing it, putting the responsibility of lifetime 
management of the memory on the developer. 

Pinning memory involves a call to `GCHandle.Alloc(<your array>, GCHandlerType.Pinned)` which returns a handle, 
and provides raw accesss to memory via `AddrOfPinnedObject()`.

While this is unusual for a C# developer, this is common practice for C/C++ developers.

It just means that as software developers, we need to consider how and when we deallocate memory in a reasonable way. 
It is no different than the requirements of any disposable resource (e.g., file handles, or window contexts). 

