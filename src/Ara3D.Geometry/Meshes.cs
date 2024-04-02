using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;
using System;

namespace Ara3D.Geometry
{
    public class TriMesh : PointsGeometry, ITriMesh, IDeformable<TriMesh>
    {
        public TriMesh(IArray<Vector3> points, IArray<Int3> faceIndices)
            : base(points)
        {
            FaceIndices = faceIndices;
        }

        public IArray<int> Indices => FaceIndices.SelectMany(f => f.ToTuple());
        public IArray<Int3> FaceIndices { get; }

        public TriMesh Deform(Func<Vector3, Vector3> f)
            => new TriMesh(Points.Select(f), FaceIndices);

        public TriMesh Transform(Matrix4x4 mat)
            => Deform(p => p.Transform(mat));

        ITriMesh ITransformable<ITriMesh>.Transform(Matrix4x4 mat)
            => Transform(mat);

        ITriMesh IDeformable<ITriMesh>.Deform(Func<Vector3, Vector3> f)
            => Deform(f);
    }

    public class QuadMesh : PointsGeometry, IQuadMesh, IDeformable<QuadMesh>
    {
        public QuadMesh(IArray<Vector3> points, IArray<Int4> faceIndices)
            : base(points)
        {
            FaceIndices = faceIndices;
        }
        public IArray<int> Indices => FaceIndices.SelectMany(f => f.ToTuple());
        public IArray<Int4> FaceIndices { get; }

        public QuadMesh Deform(Func<Vector3, Vector3> f)
            => new QuadMesh(Points.Select(f), FaceIndices);

        public QuadMesh Transform(Matrix4x4 mat)
            => Deform(p => p.Transform(mat));

        IQuadMesh ITransformable<IQuadMesh>.Transform(Matrix4x4 mat)
            => Transform(mat);

        IQuadMesh IDeformable<IQuadMesh>.Deform(Func<Vector3, Vector3> f)
            => Deform(f);
    }

    /// <summary>
    /// A type of mesh, that has the topology of a grid. Even though
    /// points might not be orthogonal, they have a forward, left, top, and right
    /// neighbour (unless on an edge).
    /// Grids may or may not have edges, for example a cylinder aligned to the Z-axis
    /// would be closed on Y.
    /// A grid mesh, may be created from a parametric surface, but can also
    /// be treated as a parametric surface.
    /// </summary>
    public class GridMesh: PointsGeometry, IQuadMesh, IParametricSurface, 
        IDeformable<GridMesh>
    {
        public GridMesh(IArray2D<Vector3> points, bool closedX, bool closedY)
            : base(points)
        {
            Points = points;
            ClosedX = closedX;
            ClosedY = closedY;
            var sd = new SurfaceDiscretization(Columns, Rows, ClosedX, ClosedY);
            FaceIndices = sd.Indices.Evaluate();
        }
        public new IArray2D<Vector3> Points { get; }
        public IArray<Int4> FaceIndices { get; }
        public bool ClosedX { get; }
        public bool ClosedY { get; }
        public int Columns => Points.Columns - 1;
        public int Rows => Points.Rows - 1;
        public IArray<int> Indices => FaceIndices.SelectMany(f => f.ToTuple());
        public Int4 GetFaceIndices(int column, int row) => FaceIndices[column + row * Columns];
        public Quad GetFace(int column, int row) => this.Face(GetFaceIndices(column, row));

        public Vector3 Eval(Vector2 uv)
        {
            Verifier.Assert(Columns >= 2);
            Verifier.Assert(Rows >= 2);
            var (lowerX, amountX) = GeometryUtil.InterpolateArraySize(Columns, uv.X, ClosedX);
            var (lowerY, amountY) = GeometryUtil.InterpolateArraySize(Rows, uv.Y, ClosedY);
            Verifier.Assert(lowerX >= 0);
            Verifier.Assert(lowerY >= 0);
            Verifier.Assert(lowerX < Columns - 1);
            Verifier.Assert(lowerY < Rows - 1);
            // TODO: the math here needs to be validated or different kinds of surfaces. 
            var quad = GetFace(lowerX, lowerY);
            return quad.Eval(((float)amountX, (float)amountY));
        }

        public GridMesh Deform(Func<Vector3, Vector3> f)
            => new GridMesh(Points.Select(f), ClosedX, ClosedY);

        public GridMesh Transform(Matrix4x4 mat)
            => Deform(p => p.Transform(mat));

        IQuadMesh ITransformable<IQuadMesh>.Transform(Matrix4x4 mat)
            => Transform(mat);

        IQuadMesh IDeformable<IQuadMesh>.Deform(Func<Vector3, Vector3> f)
            => Deform(f);
    }

    public class TesselatedMesh : PointsGeometry, IQuadMesh, IDeformable<TesselatedMesh>
    {
        public TesselatedMesh(IArray<SurfacePoint> points, IArray<Int4> faceIndices)
            : base(points.Select(p => p.Center))
        {
            Points = points;
            FaceIndices = faceIndices;
        }
        public new IArray<SurfacePoint> Points { get; }
        public IArray<int> Indices => FaceIndices.SelectMany(f => f.ToTuple());
        public IArray<Int4> FaceIndices { get; }

        public TesselatedMesh Deform(Func<Vector3, Vector3> f)
            => new TesselatedMesh(Points.Select(p => p.Deform(f)), FaceIndices);

        public TesselatedMesh Transform(Matrix4x4 mat)
            => Deform(p => p.Transform(mat));

        IQuadMesh ITransformable<IQuadMesh>.Transform(Matrix4x4 mat) 
            => Transform(mat);

        IQuadMesh IDeformable<IQuadMesh>.Deform(Func<Vector3, Vector3> f) 
            => Deform(f);
    }

    public static class Meshes
    {
        public static ITriMesh Triangle(Vector3 a, Vector3 b, Vector3 c)
            => LinqArray.Create(a, b, c).ToTriMesh();

        public static Quad Face(this IQuadMesh mesh, Int4 face)
            => (mesh.Points[face.X], 
                mesh.Points[face.Y], 
                mesh.Points[face.Z], 
                mesh.Points[face.W]);

        public static Quad Face(this IQuadMesh mesh, int face)
            => mesh.Face(mesh.FaceIndices[face]);

        public static Vector3 Eval(this Quad quad, Vector2 uv)
        {
            var lower = quad.A.Lerp(quad.B, uv.X);
            var upper = quad.D.Lerp(quad.C, uv.X);
            return lower.Lerp(upper, uv.Y);
        }

        public static QuadMesh Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            => new[] { a, b, c, d }.ToIArray().ToQuadMesh();

        public static ITriMesh ToIMesh(this AABox box)
            => PlatonicSolids.Cube.Scale(box.Extent).Translate(box.Center);

        public static readonly float Sqrt3
            = MathOps.Sqrt(3.0f);

        public static readonly float HalfSqrt3
            = Sqrt3 / 2;

        public static readonly IArray<Vector2> TrianglePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -HalfSqrt3),
                (0f, HalfSqrt3),
                (0.5f, -HalfSqrt3));

        public static TriMesh TriangleTriMesh
            = TrianglePoints.To3D().ToTriMesh();

        public static readonly IArray<Vector2> SquarePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -0.5f),
                (-0.5f, 0.5f),
                (0.5f, 0.5f),
                (0.5f, -0.5f));

        public static IArray<Vector3> To3D(this IArray<Vector2> self)
            => self.Select(x => x.ToVector3());

        public static readonly QuadMesh Square
            = SquarePoints.To3D().ToQuadMesh();

        public static TesselatedMesh TorusMesh(float r1, float r2, int uSegs, int vSegs)
            => ParametricSurfaces.Torus(r1, r2).Tesselate(uSegs, vSegs);

        public static GridMesh Extrude(this IPolyLine3D polyLine, Vector3 direction)
            => Rule(polyLine, polyLine.Translate(direction));

        public static GridMesh Extrude(this IPolyLine2D polyLine, float amount = 1.0f)
            => polyLine.To3D().Extrude(Vector3.UnitZ * amount);

        public static bool HasSameTopology(this IPolyLine2D a, IPolyLine2D b)
            => a.Points.Count == b.Points.Count && a.Closed == b.Closed;

        public static bool HasSameTopology(this IPolyLine3D a, IPolyLine3D b)
            => a.Points.Count == b.Points.Count && a.Closed == b.Closed;

        public static GridMesh Rule(this IPolyLine2D a, IPolyLine2D b)
            => a.To3D().Rule(b.To3D());

        public static GridMesh Rule(this IPolyLine3D a, IPolyLine3D b)
            => a.Points.QuadStrip(b.Points, a.Closed, false);

        public static GridMesh QuadStrip(this IArray<Vector3> lower, IArray<Vector3> upper, bool closedX, bool doubleSided)
        {
            Verifier.Assert(lower.Count == upper.Count);
            return new GridMesh(Array2D.Create(lower, upper), closedX, doubleSided);
        }
     }
}
