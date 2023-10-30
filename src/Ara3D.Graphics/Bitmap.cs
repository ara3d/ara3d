using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Math;
using Ara3D.Utils;

namespace Ara3D.Graphics
{
    public class Bitmap
    {
        public int Height { get; }
        public int Width { get; }
        public IBuffer<ColorRGBA> Pixels { get; }
        
        public Bitmap(int height, int width)
        {
            Height = height;
            Width = width;
            Pixels = new Buffer<ColorRGBA>(new ColorRGBA[Height*Width]);
        }
    }
}