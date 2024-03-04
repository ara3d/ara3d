using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public static class PrimitiveCurves
    {
        public static Curve<Vector2> ToCurve(this Func<float, Vector2> func, bool closed = true)
            => new Curve<Vector2>(func, closed);

        public static Curve<Vector3> ToCurve(this Func<float, Vector3> func, bool closed = true)
            => new Curve<Vector3>(func, closed);
    }

    public static class PrimitiveFunctions
    {
        // https://en.wikipedia.org/wiki/Trefoil_knot
        public static Func<float, Vector3> Trefoil = t => (
            t.Turns().Sin() + (2f * t).Turns().Sin() * 2f,
            t.Turns().Cos() + (2f * t).Turns().Cos() * 2f,
            -(t * 3f).Turns().Sin());

        public static Func<float, Vector2> Circle = t => 
            (t.Turns().Sin(), t.Turns().Cos());

        public static Func<Vector2, Vector3> Sphere = uv => (
            uv.X.Turns().Cos() * uv.Y.HalfTurns().Sin(),
            uv.Y.HalfTurns().Cos(),
            uv.X.Turns().Cos() * uv.Y.HalfTurns().Sin());

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/TorusBufferGeometry.js#L52
        public static Func<Vector2, Vector3> Torus = uv => (
            uv.Y.Turns().Cos() * uv.X.Turns().Cos(),
            uv.Y.Turns().Cos() * uv.X.Turns().Sin(),
            uv.Y.Sin().Divide(2));

        // https://en.wikipedia.org/wiki/Monkey_saddle
        public static Func<Vector2, float> MonkeySaddle = uv => 
            uv.X.Cube() - 3 * uv.X * uv.Y.Sqr();

        public static Func<Vector2, Vector3> Plane = uv =>
            uv.ToVector3();

        public static Func<Vector2, Vector3> Cylinder = uv =>
            uv.X.Circle().ToVector3().SetZ(uv.Y);
    }
}