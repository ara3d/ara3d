using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Collections;
using Ara3D.Math;
using Ara3D.Utils;

namespace Ara3D.Geometry
{
    public static class MeshExtensions
    {
        public static IArray<int> Indices(this IMesh mesh)
            => mesh.Faces.SelectMany(f => LinqArray.Create(f.X, f.Y, f.Z));

        // TODO: this should be in IArray
        public static IArray<float> SampleFloats(this int n, float max = 1f)
            => n == 1 ? 0f.Unit() : n.Select(i => i * max / (n - 1));

        // TODO: this should be in IArray
        public static IArray<V> CartesianProduct<T, U, V>(this IArray<T> self, IArray<U> other, Func<T, U, V> func)
            => self.SelectMany(x => other.Select(y => func(x, y)));

        public static QuadMesh Tesselate(this IParametricSurface parametricSurface, int cols, int rows = 0)
        {
            if (cols <= 0) throw new ArgumentOutOfRangeException(nameof(cols));
            if (rows <= 0) rows = cols;
            var nx = parametricSurface.ClosedX ? cols - 1 : cols;
            var ny = parametricSurface.ClosedY ? rows - 1 : rows;
            var us = nx.SampleFloats();
            var vs = ny.SampleFloats();
            var uvs = vs.CartesianProduct(us, (v, u) => new Vector2(u, v));
            var vertices = uvs.Select(parametricSurface.Eval);

            Int4 QuadMeshFaceVertices(int row, int col)
            {
                var a = row * cols + col;
                var b = row * cols + (col + 1) % cols;
                var c = (row + 1) % rows * cols + (col + 1) % cols;
                var d = (row + 1) % rows * cols + col;
                return (a, b, c, d);
            }

            var faceVertices = (rows - 1).Range().CartesianProduct((cols - 1).Range(), QuadMeshFaceVertices);
            return new QuadMesh(vertices, faceVertices);
        }

        // Computes the topology: this is a slow O(N) operation
        public static Topology ComputeTopology(this IMesh mesh)
            => new Topology(mesh);

        public static double Area(this IMesh mesh)
            => mesh.Triangles().Sum(t => t.Area);

        public static bool IsDegenerateVertexIndices(this Int3 vertexIndices)
            => vertexIndices.X == vertexIndices.Y || vertexIndices.X == vertexIndices.Z ||
               vertexIndices.Y == vertexIndices.Z;

        public static bool HasDegenerateFaceVertexIndices(this IMesh self)
            => self.Faces.Any(IsDegenerateVertexIndices);

        public static bool GeometryEquals(this IMesh mesh, IMesh other, float tolerance = Constants.Tolerance)
        {
            if (mesh.GetNumFaces() != other.GetNumFaces())
                return false;
            return mesh.Triangles().Zip(other.Triangles(), (t1, t2) => t1.AlmostEquals(t2, tolerance)).All(x => x);
        }

        public static IMesh SimplePolygonTessellate(this IEnumerable<Vector3> points)
        {
            var pts = points.ToList();
            var cnt = pts.Count;
            var sum = Vector3.Zero;
            var idxs = new List<int>(pts.Count * 3);
            for (var i = 0; i < pts.Count; ++i)
            {
                idxs.Add(i);
                idxs.Add(i + 1 % cnt);
                idxs.Add(cnt);
                sum += pts[i];
            }

            var midPoint = sum / pts.Count;
            pts.Add(midPoint);

            return pts.ToIArray().ToTriMesh(idxs.ToIArray());
        }

        /// <summary>
        /// Returns the closest point in a sequence of points
        /// </summary>
        public static Vector3 NearestPoint(this IEnumerable<Vector3> points, Vector3 x)
            => points.Minimize(float.MaxValue, p => p.DistanceSquared(x));

        /// <summary>
        /// Returns the closest point in a sequence of points
        /// </summary>
        public static Vector3 NearestPoint(this IArray<Vector3> points, Vector3 x)
            => points.ToEnumerable().NearestPoint(x);

        /// <summary>
        /// Returns the closest point in a geometry
        /// </summary>
        public static Vector3 NearestPoint(this IMesh mesh, Vector3 x)
            => mesh.Vertices.NearestPoint(x);

        public static Vector3 FurthestPoint(this IMesh mesh, Vector3 x0, Vector3 x1)
            => mesh.Vertices.FurthestPoint(x0, x1);

        public static Vector3 FurthestPoint(this IArray<Vector3> points, Vector3 x0, Vector3 x1)
            => points.ToEnumerable().FurthestPoint(x0, x1);

        public static Vector3 FurthestPoint(this IEnumerable<Vector3> points, Vector3 x0, Vector3 x1)
            => points.Maximize(float.MinValue, v => v.Distance(x0).Min(v.Distance(x1)));

        public static Vector3 FurthestPoint(this IMesh mesh, Vector3 x)
            => mesh.Vertices.FurthestPoint(x);

        public static Vector3 FurthestPoint(this IArray<Vector3> points, Vector3 x)
            => points.ToEnumerable().FurthestPoint(x);

        public static Vector3 FurthestPoint(this IEnumerable<Vector3> points, Vector3 x)
            => points.Maximize(float.MinValue, v => v.Distance(x));

        public static T SnapPoints<T>(this T mesh, float snapSize) where T: IDeformable<T>
            => snapSize.Abs() >= Constants.Tolerance
                ? mesh.Deform(v => (v * snapSize.Inverse()).Truncate() * snapSize)
                : mesh.Deform(v => Vector3.Zero);

        /// <summary>
        /// Returns the vertices organized by face corner. 
        /// </summary>
        public static IArray<Vector3> VerticesByIndex(this IMesh mesh)
            => mesh.Vertices.SelectByIndex(mesh.Indices());

        /// <summary>
        /// Returns the vertices organized by face corner, normalized to the first position.
        /// This is useful for detecting if two meshes are the same except offset by 
        /// position.
        /// </summary>
        public static IArray<Vector3> NormalizedVerticesByCorner(this IMesh m)
        {
            if (m.GetNumCorners() == 0)
                return Vector3.Zero.Repeat(0);
            var firstVertex = m.Vertices[m.Faces[0].X];
            return m.VerticesByIndex().Select(v => v - firstVertex);
        }

        /// <summary>
        /// Compares the face positions of two meshes normalized by the vertex buffer, returning the maximum distance, or null
        /// if the meshes have different topology. 
        /// </summary>
        public static float? MaxNormalizedDistance(this IMesh mesh, IMesh other)
        {
            var xs = mesh.NormalizedVerticesByCorner();
            var ys = other.NormalizedVerticesByCorner();
            if (xs.Count != ys.Count)
                return null;
            return xs.Zip(ys, (x, y) => x.Distance(y)).Max();
        }

        public static AABox BoundingBox(this IMesh mesh)
            => AABox.Create(mesh.Vertices.ToEnumerable());

        public static Sphere BoundingSphere(this IMesh mesh)
            => mesh.BoundingBox().ToSphere();

        public static float BoundingRadius(this IMesh mesh)
            => mesh.BoundingSphere().Radius;

        public static Vector3 Center(this IMesh mesh)
            => mesh.BoundingBox().Center;

        public static Vector3 Centroid(this IMesh mesh)
            => mesh.Vertices.Aggregate(Vector3.Zero, (x, y) => x + y) / mesh.Vertices.Count;

        public static bool AreIndicesValid(this IMesh mesh)
            => mesh.Indices().All(i => i >= 0 && i < mesh.Vertices.Count);

        public static bool AreAllVerticesUsed(this IMesh mesh)
        {
            var used = new bool[mesh.Vertices.Count];
            for (var i = 0; i < mesh.Indices().Count; i++)
                used[i] = true;
            return used.All(b => b);
        }

        public static IMesh ResetPivot(this IMesh mesh) 
            => mesh.Translate(-mesh.BoundingBox().CenterBottom);

        public static Triangle VertexIndicesToTriangle(this IMesh mesh, Int3 indices)
            => new Triangle(mesh.Vertices[indices.X], mesh.Vertices[indices.Y], mesh.Vertices[indices.Z]);

        public static Triangle Triangle(this IMesh mesh, int face)
            => mesh.VertexIndicesToTriangle(mesh.Faces[face]);

        public static int GetNumFaces(this IMesh mesh)
            => mesh.Faces.Count;

        public static int GetNumCorners(this IMesh mesh)
            => mesh.GetNumFaces() * 3;

        public static int GetNumVertices(this IMesh mesh)
            => mesh.Vertices.Count;

        public static int GetNumHalfEdges(this IMesh mesh)
            => mesh.GetNumCorners();

        public static int CornerToFace(this IMesh mesh, int corner)
            => corner % 3;

        public static IArray<Triangle> Triangles(this IMesh mesh)
            => mesh.GetNumFaces().Select(mesh.Triangle);

        public static IArray<Line> GetAllEdgesAsLines(this IMesh mesh)
            => mesh.Triangles().SelectMany(tri => Tuple.Create(tri.AB, tri.BC, tri.CA));

        public static IArray<Vector3> ComputedNormals(this IMesh mesh)
            => mesh.Triangles().Select(t => t.Normal);

        public static bool Planar(this IMesh mesh, float tolerance = Constants.Tolerance)
        {
            if (mesh.GetNumFaces() <= 1) return true;
            var normal = mesh.Triangle(0).Normal;
            return mesh.ComputedNormals().All(n => n.AlmostEquals(normal, tolerance));
        }

        public static IArray<Vector3> MidPoints(this IMesh mesh)
            => mesh.Triangles().Select(t => t.MidPoint);

        public static IArray<int> FacesToCorners(this IMesh mesh)
            => mesh.GetNumFaces().Select(i => i * 3);

        public static IMesh Merge(this IArray<IMesh> meshes)
        {
            var bldr = new TriMeshBuilder();
            foreach (var mesh in meshes.Enumerate())
                bldr.Add(mesh);
            return bldr.ToMesh();
        }
    }
}