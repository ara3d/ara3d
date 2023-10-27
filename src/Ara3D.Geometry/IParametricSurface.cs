using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IExplicitSurface : IProcedural<Vector2, float>, IGeometry
    {
    }

    public interface IParametricSurface : IProcedural<Vector2, Vector3>, IGeometry
    {
        bool ClosedX { get; }
        bool ClosedY { get; }
    }

    public interface IGraph2D : IProcedural<Vector2, float>
    { }

    public class ParametricSurface : Procedural<Vector2, Vector3>, IParametricSurface
    {
        public bool ClosedX { get; }
        public bool ClosedY { get; }

        public ParametricSurface(Func<Vector2, Vector3> func, bool closedX, bool closedY)
            : base(func) => (ClosedX, ClosedY) = (closedX, closedY);

        public IParametricSurface TransformInput(Func<Vector2, Vector2> f)
            => new ParametricSurface(x => Eval(f(x)), ClosedX, ClosedY);

        public IParametricSurface TransformOutput(Func<Vector3, Vector3> f)
            => new ParametricSurface(x => f(Eval(x)), ClosedX, ClosedY);

        public IGeometry Transform(Matrix4x4 mat)
            => Deform(v => v.Transform(mat));
            
        public IGeometry Deform(Func<Vector3, Vector3> f)
            => TransformOutput(f);
    }

    public class SurfacePoint
    {
        public Vector2 UV { get; }
        public Vector3 Position { get; }
        public Vector3 Normal { get; }

        public SurfacePoint(Vector2 uV, Vector3 position, Vector3 normal)
        {
            UV = uV;
            Position = position;
            Normal = normal;
        }
    }

    public static class SurfaceOperations
    {
        public static Vector3 RotateAround(Vector3 point, Vector3 axis, float angleInRad)
            => point.Transform(Quaternion.CreateFromAxisAngle(axis, angleInRad));

        public static IParametricSurface Create(this Func<Vector2, Vector3> func, bool closedOnX, bool closedOnY)
            => new ParametricSurface(func, closedOnX, closedOnY);

        public static IParametricSurface Create(this Func<float, Func<float, Vector3>> func)
            => new ParametricSurface(uv => func(uv.X)(uv.Y), false, false);

        public static IParametricSurface Sweep(this ICurve<Vector3> profile, ICurve<Vector3> path)
            => Create(uv => profile.Eval(uv.X) + path.Eval(uv.Y), profile.Closed, path.Closed);

        // https://en.wikipedia.org/wiki/Ruled_surface
        public static IParametricSurface Rule(this ICurve<Vector3> profile1, ICurve<Vector3> profile2)
            => Create(uv => profile1.Eval(uv.X).Lerp(profile2.Eval(uv.X), uv.Y), profile1.Closed && profile2.Closed, false);

        // https://en.wikipedia.org/wiki/Lathe_(graphics)
        // TODO: needs to be around a line. 
        public static IParametricSurface Revolve(this ICurve<Vector3> profile, Vector3 axis, float angleInRad, bool closed)
            => Create((uv) => profile.Eval(uv.X).RotateAround(axis, angleInRad / uv.Y), profile.Closed, closed);

        public static Vector3 LerpAlongVector(this Vector3 v, Vector3 direction, float t)
            => v.Lerp(v + direction, t);

        public static IParametricSurface Extrude(this ICurve<Vector3> profile, Vector3 vector)
            => Create((uv) => profile.Eval(uv.X).LerpAlongVector(vector, uv.Y), profile.Closed, false);

        // TODO: this needs to be finished. 
        public static IParametricSurface Loft(this ICurve<Vector3> profile, IOrientedCurve path)
            => throw new NotImplementedException();

        public static SurfacePoint GetPoint(this IParametricSurface parametricSurface, Vector2 uv)
            => new SurfacePoint(uv, parametricSurface.Eval(uv), parametricSurface.GetNormal(uv));

        public static Vector3 GetNormal(this IParametricSurface parametricSurface, Vector2 uv)
        {
            var p0 = parametricSurface.Eval(uv);
            const float eps = Constants.Tolerance;
            var p1 = parametricSurface.Eval(uv + (eps, 0));
            var p2 = parametricSurface.Eval(uv + (0, eps));
            return (p2 - p0).Cross((p1 - p0)).Normalize();
        }
    }
}