using System.Runtime.Intrinsics;

namespace Ara3D.Spans;

public class SimdMemory
{
    public readonly Vector256<byte>[] Data;
    public readonly int Length;
    public const int Width = 32;

    public SimdMemory(int length)
    {
        Length = length;
        Data = new Vector256<byte>[length / Width];
    }
}