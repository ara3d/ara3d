using System.Runtime.Intrinsics;

namespace Ara3D.Spans;

public static class SimdReader
{
    public static unsafe SimdMemory ReadAllBytes(string path, int bufferSize = 1024 * 1024)
    {
        using var fs = new FileStream(path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            false);
        
        var fileLength = fs.Length;
        if (fileLength > int.MaxValue)
            throw new IOException("File too big: > 2GB");

        var count = (int)fileLength;
        var r = new SimdMemory(count);
        fixed (Vector256<byte>* pArray = &r.Data[0])
        {
            var pBytes = (byte*)pArray;
            while (count > 0)
            {
                var span = new Span<byte>(pBytes, count);
                var n = fs.Read(span);
                if (n == 0)
                    break;
                pBytes += n;
                count -= n;
            }
        }

        return r;
    }
}