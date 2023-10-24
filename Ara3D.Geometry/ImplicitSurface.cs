using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IImplicitSurface : IProcedural<Vector3, float> { }

    public class ImplicitSurface : IImplicitSurface
    {
        public Func<Vector3, float> Func { get; }

        public ImplicitSurface(Func<Vector3, float> func)
            => Func = func;

        public float Eval(Vector3 x)
            => Func(x);
    }
}