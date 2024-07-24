namespace Ara3D.Buffers
{
    public interface ISpanSerializable<T>
    {
        int ElementSize { get; }
        T Read(ref ByteSpan span);
        int Write(ref ByteSpan span);
    }
}