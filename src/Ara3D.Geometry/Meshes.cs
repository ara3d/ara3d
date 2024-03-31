using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;

namespace Ara3D.Geometry
{
    public interface IMesh<T>
        : IPoints
    {
        IArray<T> Indices { get; }
    }

    public interface ITriMesh
        : IMesh<Int3>
    {
    }

    public interface IQuadMesh
        : IMesh<Int4>
    {
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

    /// <summary>
    /// A type of mesh, that has the topology of a grid. Even though
    /// points might not be orthogonal, they have a forward, left, top, and right
    /// neighbour (unless on an edge).
    /// Grids may or may not have edges, for example a cylinder aligned to the Z-axis
    /// would be closed on Y.
    /// A grid mesh, may be created from a parametric surface, but can also
    /// be treated as a parametric surface.
    /// </summary>
    public class GridMesh: PointsGeometry, IQuadMesh, IParametricSurface
    {
        public GridMesh(IArray2D<Vector3> points, bool closedX, bool closedY)
            : base(points)
        {
            Points = points;
            ClosedX = closedX;
            ClosedY = closedY;
            var sd = new SurfaceDiscretization(Columns, Rows, ClosedX, ClosedY);
            Indices = sd.Indices.Evaluate();
        }
        public new IArray2D<Vector3> Points { get; }
        public IArray<Int4> Indices { get; }
        public bool ClosedX { get; }
        public bool ClosedY { get; }
        public int Columns => Points.Columns;
        public int Rows => Points.Rows;
        public Int4 GetFaceIndices(int column, int row) => Indices[column + row * Columns];
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
    }

    public class TesselatedMesh : PointsGeometry, IQuadMesh
    {
        public TesselatedMesh(IArray<SurfacePoint> points, IArray<Int4> faceIndices)
            : base(points.Select(p => p.Center))
        {
            Points = points;
            Indices = faceIndices;
        }
        public new IArray<SurfacePoint> Points { get; }
        public IArray<Int4> Indices { get; }
    }

    public static class Meshes
    {
        public static TriMesh ToTriMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => ToTriMesh(vertices, (indices ?? vertices.Indices()).SelectTriplets((a, b, c) => new Int3(a, b, c)));

        public static TriMesh ToTriMesh(this IArray<Vector3> vertices, IArray<Int3> indices)
            => new TriMesh(vertices, indices);

        public static QuadMesh ToQuadMesh(this IArray<Vector3> vertices, IArray<int> indices = null)
            => ToQuadMesh(vertices,
                (indices ?? vertices.Indices()).SelectQuartets((a, b, c, d) => new Int4(a, b, c, d)));

        public static QuadMesh ToQuadMesh(this IArray<Vector3> vertices, IArray<Int4> indices)
            => new QuadMesh(vertices, indices);

        public static TriMesh Triangle(Vector3 a, Vector3 b, Vector3 c)
            => ToTriMesh(new[] { a, b, c }.ToIArray());

        public static Quad Face(this IQuadMesh mesh, Int4 face)
            => (mesh.Points[face.X], mesh.Points[face.Y], mesh.Points[face.Z], mesh.Points[face.W]);

        public static Quad Face(this IQuadMesh mesh, int face)
            => mesh.Face(mesh.Indices[face]);

        public static Vector3 Eval(this Quad quad, Vector2 uv)
        {
            var lower = quad.A.Lerp(quad.B, uv.X);
            var upper = quad.D.Lerp(quad.C, uv.X);
            return lower.Lerp(upper, uv.Y);
        }

        public static QuadMesh Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            => ToQuadMesh(new[] { a, b, c, d }.ToIArray());

        public static QuadMesh Cube
            => Square.Translate(-Vector3.UnitZ / 2).Points.Concat(
                    Square.Translate(Vector3.UnitZ / 2).Points)
                .ToQuadMesh(new Int4[]
                {
                    (0, 1, 2, 3),
                    (1, 5, 6, 2),
                    (7, 6, 5, 4),
                    (4, 0, 3, 7),
                    (4, 5, 1, 0),
                    (3, 2, 6, 7),
                }.ToIArray());

        public static QuadMesh ToIMesh(this AABox box)
            => Cube.Scale(box.Extent).Translate(box.Center);

        public static float Sqrt2
            = MathOps.Sqrt(2.0f);

        public static float Sqrt3
            = MathOps.Sqrt(3.0f);

        public static float HalfSqrt3
            = Sqrt3 / 2;

        public static readonly ITriMesh Tetrahedron
            = ToTriMesh(new Vector3[]
                {
                    (1f, 0.0f, -1f / Sqrt2),
                    (-1f, 0.0f, -1f / Sqrt2),
                    (0.0f, 1f, 1f / Sqrt2),
                    (0.0f, -1f, 1f / Sqrt2)
                }.ToIArray(),
                LinqArray.Create<Int3>((0, 1, 2), (1, 0, 3), (0, 2, 3), (1, 3, 2)));

        public static IArray<Vector2> TrianglePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -HalfSqrt3),
                (0f, HalfSqrt3),
                (0.5f, -HalfSqrt3));

        public static TriMesh TriangleTriMesh
            = TrianglePoints.To3D().ToTriMesh();

        public static IArray<Vector2> SquarePoints
            = LinqArray.Create<Vector2>(
                (-0.5f, -0.5f),
                (-0.5f, 0.5f),
                (0.5f, 0.5f),
                (0.5f, -0.5f));

        public static IArray<Vector3> To3D(this IArray<Vector2> self)
            => self.Select(x => x.ToVector3());

        public static readonly QuadMesh Square
            = SquarePoints.To3D().ToQuadMesh();

        public static readonly TriMesh Octahedron
            = Square
                .Points
                .Append(Vector3.UnitZ / 2, -Vector3.UnitZ / 2)
                .Normalize()
                .ToTriMesh(
                    LinqArray.Create<Int3>(
                        (0, 1, 4), (1, 2, 4), (2, 3, 4),
                        (3, 2, 5), (2, 1, 5), (1, 0, 5)));

        private static readonly float _t = (float)((1.0 + MathOps.Sqrt(5.0)) / 2.0);
        private static readonly float _rt = MathOps.Inverse(_t);

        // https://mathworld.wolfram.com/RegularDodecahedron.html
        // https://github.com/mrdoob/three.js/blob/master/src/geometries/DodecahedronGeometry.js
        public static IArray<Vector3> DodecahedronPoints = LinqArray.Create<Vector3>(
            // (±1, ±1, ±1)
            (-1, -1, -1), (-1, -1, 1),
            (-1, 1, -1), (-1, 1, 1),
            (1, -1, -1), (1, -1, 1),
            (1, 1, -1), (1, 1, 1),

            // (0, ±1/φ, ±φ)
            (0, -_rt, -_t), (0, -_rt, _t),
            (0, _rt, -_t), (0, _rt, _t),

            // (±1/φ, ±φ, 0)
            (-_rt, -_t, 0), (-_rt, _t, 0),
            (_rt, -_t, 0), (_rt, _t, 0),

            // (±φ, 0, ±1/φ)
            (-_t, 0, -_rt), (_t, 0, -_rt),
            (-_t, 0, _rt), (_t, 0, _rt));

        public static TriMesh Dodecahedron = DodecahedronPoints.ToTriMesh(LinqArray.Create<Int3>(
            (3, 11, 7), (3, 7, 15), (3, 15, 13),
            (7, 19, 17), (7, 17, 6), (7, 6, 15),
            (17, 4, 8), (17, 8, 10), (17, 10, 6),
            (8, 0, 16), (8, 16, 2), (8, 2, 10),
            (0, 12, 1), (0, 1, 18), (0, 18, 16),
            (6, 10, 2), (6, 2, 13), (6, 13, 15),
            (2, 16, 18), (2, 18, 3), (2, 3, 13),
            (18, 1, 9), (18, 9, 11), (18, 11, 3),
            (4, 14, 12), (4, 12, 0), (4, 0, 8),
            (11, 9, 5), (11, 5, 19), (11, 19, 7),
            (19, 5, 14), (19, 14, 4), (19, 4, 17),
            (1, 12, 14), (1, 14, 5), (1, 5, 9)));

        public static IArray<Vector3> IcosahedronPoints =
            LinqArray.Create<Vector3>(
                (-1f, _t, 0.0f),
                (1f, _t, 0.0f),
                (-1f, -_t, 0.0f),
                (1f, -_t, 0.0f),
                (0.0f, -1f, _t),
                (0.0f, 1f, _t),
                (0.0f, -1f, -_t),
                (0.0f, 1f, -_t),
                (_t, 0.0f, -1f),
                (_t, 0.0f, 1f),
                (-_t, 0.0f, -1f),
                (-_t, 0.0f, 1f)).Select(v => v.Normalize());

        public static TriMesh Icosahedron =
            IcosahedronPoints.ToTriMesh(
                LinqArray.Create<Int3>((0, 11, 5), (0, 5, 1), (0, 1, 7), (0, 7, 10), (0, 10, 11),
                    (1, 5, 9), (5, 11, 4), (11, 10, 2), (10, 7, 6), (7, 1, 8),
                    (3, 9, 4), (3, 4, 2), (3, 2, 6), (3, 6, 8), (3, 8, 9),
                    (4, 9, 5), (2, 4, 11), (6, 2, 10), (8, 6, 7), (9, 8, 1)));

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

        /// <summary>
        /// Creates a quad-mesh from a a quad strip.   
        /// </summary>
        public static GridMesh QuadStrip(this IArray<Vector3> lower, IArray<Vector3> upper, bool closedX, bool doubleSided)
        {
            Verifier.Assert(lower.Count == upper.Count);
            return new GridMesh(Array2D.Create(lower, upper), closedX, doubleSided);
        }
     }
}
