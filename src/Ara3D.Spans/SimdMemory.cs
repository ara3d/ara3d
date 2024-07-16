using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Ara3D.Spans;

/// <summary>
/// Allocates fixed and aligned memory for usage with SIMD Vectors  
/// </summary>
public unsafe class SimdMemory : IDisposable
{
    public readonly Vector256<byte>* VectorPtr;
    public readonly Byte* BytePtr;
    public readonly nint AllocPtr;
    public readonly int NumBytes;
    public readonly int NumVectors;
    public const int Width = 32;

    public SimdMemory(int numBytes)
    {
        NumBytes = numBytes;
        NumVectors = (numBytes / Width) + (numBytes % Width == 0 ? 0 : 1);
        
        // This is the amount of space we need for the vectors
        // We add 1 to number of vectors to account for alignment 
        var space = (NumVectors + 1) * Width;
        AllocPtr = Marshal.AllocHGlobal(space + Width);

        var alignedPtr = (nint)(((long)AllocPtr + Width - 1) & ~(Width - 1));
        VectorPtr = (Vector256<byte>*)alignedPtr;
        BytePtr = (byte*)VectorPtr;
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(AllocPtr);
    }
}