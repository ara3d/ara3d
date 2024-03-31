using System;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public interface IPolyLine<T>
    {
        bool Closed { get; }
        IArray<T> Points { get; }
        ILineSegment<T> Segment(int i);
    }

    public interface IPolyLine2D : IPolyLine<Vector2>
    { }

    public interface IPolyLine3D : IPolyLine<Vector3>, ITransformable, IDeformable
    { }
    
    public abstract class PolyLine<T> : IPolyLine<T>
    {
        public PolyLine(IArray<T> points, bool closed)
            => (Points, Closed) = (points, closed);
        public bool Closed { get; }
        public IArray<T> Points { get; }
        public abstract ILineSegment<T> Segment(int i);
     }

    public class PolyLine2D : PolyLine<Vector2>, IPolyLine2D
    {
        public PolyLine2D(IArray<Vector2> points, bool closed)
            : base(points, closed) { }
            
        public override ILineSegment<Vector2> Segment(int i)
            => new LineSegment2D(this.Point(i), this.Point(i + 1));
    }

    public class PolyLine3D : PolyLine<Vector3>, IPolyLine3D
    {
        public PolyLine3D(IArray<Vector3> points, bool closed)
            : base(points, closed) { }

        public override ILineSegment<Vector3> Segment(int i)
            => new LineSegment3D(this.Point(i), this.Point(i + 1));

        public ITransformable TransformImpl(Matrix4x4 mat)
            => new PolyLine3D(Points.Transform(mat), Closed);

        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new PolyLine3D(Points.Select(f), Closed);
    }

    public static class PolyLineExtensions
    {
        public static int NumSegments<T>(this IPolyLine<T> self)
            => self.Points.Count - 1 + (self.Closed ? 1 : 0);

        public static T Point<T>(this IPolyLine<T> self, int i)
            => self.Points.ElementAtModulo(i);

        public static IArray<ILineSegment<T>> Segments<T>(this IPolyLine<T> self)
            => self.NumSegments().Select(self.Segment);

        public static PolyLineCurve<T> Curve<T>(this IPolyLine<T> self) 
            => new PolyLineCurve<T>(self);

        public static IPolyLine3D To3D(this IPolyLine2D self)
            => new PolyLine3D(self.Points.Select(p => p.ToVector3()), self.Closed);
    }
}