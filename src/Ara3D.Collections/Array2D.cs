﻿using System;

namespace Ara3D.Collections
{
    public interface IArray2D<T>
        : IArray<T>
    {
        int Columns { get; }
        int Rows { get; }
        T this[int column, int row] { get; }
    }

    public class Array2D<T> : IArray2D<T>
    {
        public int Columns { get; }
        public int Rows { get; }
        public T this[int column, int row] => this[row * Columns + column];
        public IArray<T> Data { get; }
        public T this[int index] => Data[index];
        public int Count => Data.Count;

        public IIterator<T> Iterator => Data.Iterator;

        public Array2D(IArray<T> data, int rows, int columns)
        {
            if (rows * columns != data.Count)
                throw new Exception($"The data array has length {data.Count} but expected {rows * columns}");
            Rows = rows;
            Columns = columns;
            Data = data;
        }
    }

    public static class Array2D
    {
        public static IArray2D<T> Create<T>(params IArray<T>[] arrays)
            => arrays.ToIArray().ToArray2D();
        
        public static IArray2D<T> ToArray2D<T>(this IArray<IArray<T>> arrays)
            => new Array2D<T>(arrays.SelectMany(a => a), arrays.Count, arrays[0].Count);

        public static IArray2D<T> ToArray2D<T>(this IArray<T> array, int rows, int columns)
            => new Array2D<T>(array, rows, columns);

        public static IArray<T> GetRow<T>(this IArray2D<T> self, int row)
            => self.SubArray(row * self.Columns, self.Columns);

        public static IArray<T> GetColumn<T>(this IArray2D<T> self, int column)
            => self.Stride(column, self.Columns); 
    }
}
