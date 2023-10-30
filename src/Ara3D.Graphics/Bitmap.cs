using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Graphics
{
    [Mutable]
    public class Bitmap : IBitmap
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

        public ColorRGBA Eval(int x, int y)
            => GetPixel(x + y * Width);

        public ColorRGBA GetPixel(int i)
            => PixelBuffer.GetTypedData()[i];
    }
}