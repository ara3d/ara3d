# LinqArray

**LinqArray** is a pure functional .NET library from **[VIM AEC](https://vimaec.com)** that provides LINQ functionality for 
immutable (read only) arrays, rather than `IEnumerable` (which is effectively a stream), 
while preserving `O(1)` complexity when retrieving the count or items by index. 
It is performant, memory efficient, cross-platform, safe, and easy to use.

## Overview 

LinqArray is a set of extension methods build on the `IArray<T>` interface: 

```
interface IArray<T> {
    int Count { get; }
    T this[int n] { get; }
}
```

Because the interface does not mutate objects it is safe to use in a multithreaded context. 
Furthermore, as with regular LINQ for `IEnumerable`, evaluation of many of the operations can 
be performed on demand (aka lazily). 

This allows for algorithms to be constructed that do not load large data sets into memory.  

## Motivation

There are two key characteristics of an array data type which are important to maintain in some problem domains: 
1. Retrieving size of collection in `O(1)` 
2. Retrieving any item in collection by index in `O(1)` 

LINQ provides a large library of extremely useful and powerful algorithms that work on any data type that can be enumerated. 
However most LINQ methods return an `IEnumerable` interface which has `O(N)` time for retrieving the size of a collection 
and `O(N)` time for retrieving  an element in the collection at a given index. 

Using the `IArray` extension methods in `LinqArray`, all of the properties of the array can be maintained through a number 
of operations, include `Select`, `Take`, `Reverse`, and more, as well as various useful array specific operations like
`Slice`.

### Note about Big O Complexity 

The notation `O(1)` is called Big-O notation and describes the average running time of an operation in terms of the size of the input set. 
The `O(1)` means that the running time of the operation is a fixed constant independent of the size of the collection.  

## Similar Work

This library is based on an article on CodeProject.com called [LINQ for Immutable Arrays](https://www.codeproject.com/Articles/517728/LINQ-for-Immutable-Arrays). 

Unlike [LinqFaster](https://github.com/jackmott/LinqFaster) by Jack Mott, the evaluation of functions in 
`LinqArray` happen on demand (lazily), 
and have no side effects. This means that this library can be easily used in a multi-threaded context without 
inccurring the overhead and complexity of synchronization. 