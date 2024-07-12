    namespace Ara3D.Spans;

public unsafe class FixedList<T> where T : unmanaged
{
    public readonly int Capacity;
    public int Count;
    public readonly T* Data;

    public FixedList(T* data, int capacity)
    {
        Data = data;
        Capacity = capacity;
    }

    public void Add(in T item)
        => Data[Count++] = item;
}