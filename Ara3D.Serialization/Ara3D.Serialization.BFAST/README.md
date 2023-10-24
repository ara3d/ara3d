# BFAST

[<img src="https://img.shields.io/nuget/v/Vim.Bfast.svg">](https://www.nuget.org/packages/Vim.Bfast) 

BFAST stands for the **B**inary **F**ormat for **A**rray **S**erialization and **T**ransmission.

## Summary

BFAST is an extremely efficent and simple alternative to ProtoBuf and FlatBuffers, whe data that follows the form
of a collection of name/value pairs where names are strings, and values are arrays of bytes.

* Unlike JSON, XML, and YAML: BFAST is binary
* Unlike ProtoBuf and FlatBuffers: BFAST does not require a schema 
* Unlike TAR: BFAST is simple and easy to implement
* Unlike ZIP: BFAST is not concerned with compression 

## Details 

BFAST is a data format for simple and efficient serialization and deserialization of 
collections of named data buffers in a generic and cross-platform manner. 
A BFAST data buffer is a named arrays of binary data (bytes) that is aligned on 64 byte boundaries. 

You would use tshe BFAST structure if you have a binary data to serialize that is mostly in the form of 
long arrays. For example a set of files that you want to bundle together without wanting to bring in 
the overhead of a compression library or re-implementing TAR. We use BFAST to encode mesh data and as 
containers for other data.

BFAST is intended to be a high-performance implementation that is fast enough to use as a purely 
in-memory low-level data format, for representing arbitrary data such as meshes, point-clouds, image data, 
collections of files, etc. and to scale to data that must be processed out of core. One of the design goals was to assure 
that the format could be easily and efficiently decoded using JavaScript on most modern web-browsers
with very little code.

BFAST is maintained by [VIMaec LLC](https://www.vimaec.com) and is licensed under the terms of 
the MIT License.

## Features

* Very small implementation overhead 
* Easy to implement efficient and conformant encoders and decoders in different languages 
* Fast random access to any point in the data format with a minimum of disk accesses
* Format and endianess easily identified through a magic number at the front of the file
* Data arrays are 64 byte aligned to facilitate casting to SIMD data types (eg. AVX-512)
* Array offsets are encoded using 64-bit integers to supports large data sets
* Positions of data buffers are encoded in the beginning of the file
* Quick and easy to validate that a block is a valid BFAST encoding of data

## Rationale

Encoding containers of binary data is a deceptively simple problem that is easy to solve
in ways that have are not as efficient of generic as possible, or dependent on a particular platform. 
We proposing a standardized solution to the problem in the form of a specification and sample 
implementation that can allow software to easily encode low level binary data in a manner 
that is both efficient and cross-platform. 

## Related Libraries 

The following is a partial list of commonly used binary data serialization formats:

* [Protcol Buffers](https://developers.google.com/protocol-buffers/)
* [FlatBuffers](https://github.com/google/flatbuffers)
* [BINN](https://github.com/liteserver/binn/)
* [BSON](http://bsonspec.org/)
* [UBJSON](http://ubjson.org/)
* [MessagePack](https://msgpack.org/)
* [CBOR](https://cbor.io/)
* [TAR](https://www.gnu.org/software/tar/manual/html_node/Standard.html)

For a more comprehensive list see:

* https://en.wikipedia.org/wiki/Comparison_of_data-serialization_formats
* https://en.wikipedia.org/wiki/List_of_archive_formats

# Specification

See the file [spec.txt](spec.txt) for the official specification.
