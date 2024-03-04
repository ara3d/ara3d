using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public interface ITriMesh
        : IPoints
    {
        IArray<Int3> Indices { get; }
    }

    public interface IQuadMesh
        : IPoints
    {
        IArray<Int4> Indices { get; }
    }

    public class TriMesh : PointsGeometry, ITriMesh
    {
        public TriMesh(IArray<Vector3> points, IArray<Int3> faceIndices)
            : base(points)
        {
            Indices = faceIndices;
        }
        public IArray<Int3> Indices { get; }
    }

    public class QuadMesh : PointsGeometry, IQuadMesh
    {
        public QuadMesh(IArray<Vector3> points, IArray<Int4> faceIndices)
            : base(points)
        {
            Indices = faceIndices;
        }
        public IArray<Int4> Indices { get; }
    }

    public class TesselatedMesh : PointsGeometry, IQuadMesh
    {
        public TesselatedMesh(IArray<SurfacePoint> points, IArray<Int4> faceIndices)
            : base(points.Select(p => p.Position))
        {
            Points = points;
            Indices = faceIndices;
        }
        public IArray<SurfacePoint> Points { get; }
        public IArray<Int4> Indices { get; }
    }
}
