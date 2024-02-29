using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface ITriMesh
        : IPoints
    {
        IArray<Int3> FaceIndices { get; }
    }

    public interface IQuadMesh
        : IPoints
    {
        IArray<Int4> FaceIndices { get; }
    }

    public class TriMesh : Points, ITriMesh
    {
        public TriMesh(IArray<Vector3> points, IArray<Int3> faceIndices)
            : base(points)
        {
            FaceIndices = faceIndices;
        }
        public IArray<Int3> FaceIndices { get; }
    }

    public class QuadMesh : Points, IQuadMesh
    {
        public QuadMesh(IArray<Vector3> points, IArray<Int4> faceIndices)
            : base(points)
        {
            FaceIndices = faceIndices;
        }
        public IArray<Int4> FaceIndices { get; }
    }

}
