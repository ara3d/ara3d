using Ara3D.Math;
using System;

namespace Ara3D.Geometry
{
    public interface ICurve : ICurve<Vector3> { }

    public class Curve<T> : Procedural<float, T>, ICurve<T>
    {
        public Curve(Func<float, T> func, bool closed)
            : base(func)
            => Closed = closed;

        public bool Closed { get; }
    }

    public interface ICurve<T> : IProcedural<float, T>
    {
        bool Closed { get; }
    }

    public interface ICurve2D : ICurve<Vector2> { }

    public interface IOrientedCurve : ICurve<PointNormal> { }

    public class Curve3D : Procedural3D<float>, ICurve
    {
        public Curve3D(Func<float, Vector3> func, bool closed) : base(func)
        {
            Closed = closed;
        }

        public bool Closed { get; }
    }
}