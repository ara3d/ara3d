﻿using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IBoundedRange { }
    public interface IRestrictedDomain { }

    public class PointNormal
    {
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public PointNormal(Vector3 point, Vector3 normal)
            => (Point, Normal) = (point, normal);
    }

    public interface IProcedural<TIn, TOut> 
    {
        TOut Eval(TIn x);
    }

    public interface ICurve<T> : IProcedural<float, T>
    {
        bool Closed { get; }
    }

    public interface ICurve2D : ICurve<Vector2> { }
    public interface ICurve3D : ICurve<Vector3> { }

    public interface IOrientedCurve : ICurve<PointNormal> { }

    public interface IColorGradient : IProcedural<float, Vector4> { }

    public interface IDistanceField2D : IProcedural<Vector2, float> { }
    public interface IVectorField2D : IProcedural<Vector2, Vector2> { }


    public interface IVolume : IProcedural<Vector3, bool> { }
    public interface IDistanceField3D : IProcedural<Vector3, float> { }
    public interface IProceduralVectorField3D : IProcedural<Vector3, Vector3> { }

    public interface IMask : IProcedural<Vector2, bool> { }
    public interface IField : IProcedural<Vector2, float> { }
    public interface IBump : IProcedural<Vector2, Vector3> { }
    public interface IProceduralTexture : IProcedural<Vector2, Vector4> { }

    public class Procedural<T1, T2> : IProcedural<T1, T2>
    {
        private readonly Func<T1, T2> _func;
        public Procedural(Func<T1, T2> func) => _func = func;
        public T2 Eval(T1 x) => _func(x);
    }

    public static class Procedurals
    {
        public static IProcedural<T1, T2> ToProcedural<T1,T2>(this Func<T1, T2> f)
            => new Procedural<T1, T2>(f);

        public static IProcedural<T1, T4> Zip<T1, T2, T3, T4>(
            this IProcedural<T1, T2> p1, 
            IProcedural<T1, T3> p2,
            Func<T2, T3, T4> f)
            => ToProcedural<T1, T4>(x => f(p1.Eval(x), p2.Eval(x)));

        public static IProcedural<T1, T3> Select<T1, T2, T3>(
            this IProcedural<T1, T2> self, 
            Func<T2, T3> f)
            => ToProcedural<T1, T3>(x => f(self.Eval(x)));
        
        public static IProcedural<T3, T2> Remap<T1, T2, T3>(
            this IProcedural<T1, T2> self, 
            Func<T3, T1> f)
            => ToProcedural<T3, T2>(x => self.Eval(f(x)));
    }
}
