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
        //https://mathoverflow.net/questions/91444/what-is-parameterization-of-the-trefoil-knot-surface-in-r%C2%B3#:~:text=Start%20with%20the%20parametrization%20of,that%20sits%20inside%20the%20trefoil.
        public static Func<float, Vector3> TrefoilKnot
            => TorusKnot(2, 3);

        // https://en.wikipedia.org/wiki/Torus_knot
        public static Func<float, Vector3> TorusKnot(int p, int q)
            => t =>
            {
                var r = (q * t.Turns()).Cos() + 2;
                var x = r * (p * t.Turns()).Cos();
                var y = r * (p * t.Turns()).Sin();
                var z = -(q * t.Turns()).Sin();
                return (x, y, z);
            };

        // https://en.wikipedia.org/wiki/Trefoil_knot
        public static Func<float, Vector3> Trefoil = t => (
            t.Turns().Sin() + (2f * t).Turns().Sin() * 2f,
            t.Turns().Cos() + (2f * t).Turns().Cos() * 2f,
            -(t * 3f).Turns().Sin());

        public static Func<float, Vector2> Circle = t => 
            (t.Turns().Sin(), t.Turns().Cos());

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/SphereBufferGeometry.js#L76
        public static Func<Vector2, Vector3> Sphere = uv => (
            -uv.X.Turns().Cos() * uv.Y.Turns().Sin(),
            uv.Y.Turns().Cos(),
            uv.X.Turns().Sin() * uv.Y.Turns().Sin());

        // see: https://github.com/mrdoob/three.js/blob/9ef27d1af7809fa4d9943f8d4c4644e365ab6d2d/src/geometries/TorusBufferGeometry.js#L52
        public static Vector3 Torus(Vector2 uv, float radius, float tube)
        {
            uv *= Constants.TwoPi;
            return new Vector3(
                (radius + tube * uv.Y.Cos()) * uv.X.Cos(),
                (radius + tube * uv.Y.Cos()) * uv.X.Sin(),
                tube * uv.Y.Sin());
        }

        // https://en.wikipedia.org/wiki/Monkey_saddle
        public static Func<Vector2, float> MonkeySaddle = uv => 
            uv.X.Cube() - 3 * uv.X * uv.Y.Sqr();

        public static Func<Vector2, Vector3> Plane = uv =>
            uv.ToVector3();

        public static Func<Vector2, Vector3> Disc = uv =>
            uv.X.Circle() * uv.Y;

        public static Func<Vector2, Vector3> Cylinder = uv =>
            uv.X.Circle().ToVector3().SetZ(uv.Y);

        public static Vector3 ConicalSection(Vector2 uv, float bottomRadius, float topRadius) 
            => (uv.X.Circle() * bottomRadius.Lerp(topRadius, uv.Y)).ToVector3().SetZ(uv.Y);

    }
}