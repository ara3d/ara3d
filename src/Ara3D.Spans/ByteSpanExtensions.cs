using System.Runtime.CompilerServices;
using System.Text;

namespace Ara3D.Spans
{
    public static class ByteSpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ByteSpan ToByteSpanPinned(this string str)
            => Encoding.ASCII.GetBytes(str).ToByteSpanPinned();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ByteSpan ToByteSpanPinned(this byte[] bytes)
            => ByteSpan.CreatePermanent(bytes);
    }
}