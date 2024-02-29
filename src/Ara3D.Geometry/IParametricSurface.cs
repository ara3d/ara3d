using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    /// <summary>
    /// A surface is a continuous 3-dimensional shape with no volume.
    /// Some examples of surfaces could be a sphere, cylinder, or torus.  
    /// </summary>
    public interface ISurface : IGeometry
    {
        bool ClosedX { get; }
        bool ClosedY { get; }
    }

    /// <summary>
    /// A bounded surface is a 2D sampling of a surface within some 
    /// </summary>
    public interface IBoundedSurface : ISurface
    {
        AABox2D Bounds { get; }
        ISurface Surface { get; }
    }

    public class BoundedSurface : IBoundedSurface
    {
        public bool ClosedX => false;
        public bool ClosedY => false;
        public AABox2D Bounds { get; }
        public ISurface Surface { get; }

        public BoundedSurface(ISurface surface, AABox2D bounds)
            => (Bounds, Surface) = (bounds, surface);

        public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(v => v.Transform(mat));

        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new BoundedSurface(Surface.Deform(f), Bounds);
    }

    /// <summary>
    /// An explicit surface is a function mapping UV coordinates
    /// to real-numbers. It can be trivially converted to a parametric surface.
    /// One application is as a height map.
    /// </summary>
    public interface IExplicitSurface : IProcedural<Vector2, float>, ISurface
    {
    }

    /// <summary>
    /// A parametric surface maps UV coordinates to 3-dimensional points in space.
    /// Any explicit surface can be combined with a parametric surface by interpreting
    /// the normals.  
    /// </summary>
    public interface IParametricSurface : IProcedural<Vector2, Vector3>, ISurface
    {
    }

    public class ParametricSurface : Procedural<Vector2, Vector3>, IParametricSurface
    {
        public bool ClosedX { get; }
        public bool ClosedY { get; }

        public ParametricSurface(Func<Vector2, Vector3> func, bool closedX, bool closedY)
            : base(func) => (ClosedX, ClosedY) = (closedX, closedY);

        public IParametricSurface TransformInput(Func<Vector2, Vector2> f)
            => new ParametricSurface(x => Eval(f(x)), ClosedX, ClosedY);

       public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(v => v.Transform(mat));
            
        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new ParametricSurface(x => f(Eval(x)), ClosedX, ClosedY);
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

        public static IParametricSurface Extrude(this ICurve profile, Vector3 vector)
            => Create((uv) => profile.Eval(uv.X).LerpAlongVector(vector, uv.Y), profile.Closed, false);

        // TODO: this needs to be finished. 
        public static IParametricSurface Loft(this ICurve profile, IOrientedCurve path)
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

        public static IParametricSurface ToParametricSurface(this IExplicitSurface self)
            => new ParametricSurface(uv => uv.ToVector3().SetX(self.Eval(uv)), false, false);
    }

    public static class CurveOperations
    {
        public static ICurve ToCurve3D(this ICurve2D curve, bool closed = false, int axis = 2)
        {
            switch (axis)
            {
                case 0: return new Curve3D(t => curve.Eval(t).ToVector3().ZXY, closed);
                case 1: return new Curve3D(t => curve.Eval(t).ToVector3().XZY, closed);
                case 2: return new Curve3D(t => curve.Eval(t).ToVector3(), closed);
                default: throw new ArgumentOutOfRangeException(nameof(axis));
            }
        }
    }
}