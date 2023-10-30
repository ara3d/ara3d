using System.Diagnostics;
using System.Linq.Expressions;
using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Math;
using Ara3D.Utils;
using System.Runtime.InteropServices;
using Ara3D.Utils.Unsafe;

namespace Ara3D.Graphics
{
    public class Bitmap
    {
        public int Height { get; }
        public int Width { get; }
        public Buffer<ColorRGBA> PixelBuffer { get; }
        public Bitmap(int width, int height)
        {
            Height = height;
            Width = width;
            PixelBuffer = new Buffer<ColorRGBA>(new ColorRGBA[Height*Width]);
        }
        
        public int GetNumPixels()
            => Width * Height;

        public void SetPixel(int x, int y, ColorRGBA color)
            => SetPixel(x + y * Width, color);

        public void SetPixel(int i, ColorRGBA color)
            => PixelBuffer.GetTypedData()[i] = color;

        public ColorRGBA GetPixel(int x, int y)
            => GetPixel(x + y * Width);

        public ColorRGBA GetPixel(int i)
            => PixelBuffer.GetTypedData()[i];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BitmapHeader
    {
        public ushort Magic; 
        public uint Size;
        public ushort Unused1;
        public ushort Unused2;
        public uint DataStartOffset;  
        
        public static uint StructSize = 14;

        public unsafe BitmapHeader(uint dataSize)
        {
            Magic = 0x4D42;
            DataStartOffset = StructSize + DibHeader.StructSize;
            Size = dataSize + DataStartOffset;
            Unused1 = default;
            Unused2 = default;
            Debug.Assert(sizeof(BitmapHeader) == StructSize);
        }
    }

    // https://en.wikipedia.org/wiki/BMP_file_format
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DibHeader
    {
        public uint biSize;
        public uint biWidth;
        public uint biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public uint biXPelsPerMeter;
        public uint biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public const uint StructSize = 40;

        // Uncompressed 32 bit. 
        public DibHeader(int width, int height, uint dpi = 600)
        {
            const double inchPerMeter = 39.3701;
            uint pixPerMeter = (uint)(dpi * inchPerMeter);
            biSize = StructSize;
            biWidth = (uint)width;
            biHeight = (uint)height;
            biPlanes = 1;
            biBitCount = 32;
            biCompression = 0;
            biSizeImage = (uint)width * (uint)height * 4;
            biXPelsPerMeter = pixPerMeter;
            biYPelsPerMeter = pixPerMeter;
            biClrUsed = 0;
            biClrImportant = 0;
            Debug.Assert(sizeof(DibHeader) == StructSize);
        }
    }

    public static class BitmapUtil
    {
        public static void Save(this Bitmap bitmap, FilePath path)
        {
            var writer = path.OpenWrite();
            var dibHeader = new DibHeader(bitmap.Width, bitmap.Height);
            var bmpHeader = new BitmapHeader(dibHeader.biSizeImage);
            writer.WriteValue(bmpHeader);
            Debug.Assert(writer.Position == 14);
            writer.WriteValue(dibHeader);
            Debug.Assert(writer.Position == 54);
            writer.Write(bitmap.PixelBuffer);
            var n = bitmap.PixelBuffer.GetNumBytes();
            Debug.Assert(n == bitmap.Width * bitmap.Height * 4);
            Debug.Assert(dibHeader.biSizeImage == n);
            Debug.Assert(bmpHeader.Size == 54 + n);
            writer.Close();
        }
    }
}