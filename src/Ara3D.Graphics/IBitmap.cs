﻿using Ara3D.Math;

namespace Ara3D.Graphics
{
    public interface IBitmap
    {
        int Width { get; }
        int Height { get; }
        ColorRGBA Eval(int x, int y);
    }
}