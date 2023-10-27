using System;

namespace Ara3D.Geometry
{
    public class Curve<T> : Procedural<float, T>, ICurve<T>
    {
        public Curve(Func<float, T> func, bool closed)
            : base(func)
            => Closed = closed;

        public bool Closed { get; }
    }
}