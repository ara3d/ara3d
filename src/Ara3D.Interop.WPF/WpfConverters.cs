using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Math;
using Ara3D.Serialization.G3D;

namespace Ara3D.Interop.WPF;

public static class WpfConverters
{
    public static DVector3 ToAra3D(this Point3D p)
        => (p.X, p.Y, p.Z);

    public static DVector3 ToAra3D(this Vector3D v)
        => (v.X, v.Y, v.Z);

    public static Vector3D ToWpf(this DVector3 v)
        => new Vector3D(v.X, v.Y, v.Z);

    public static Vector3D ToWpf(this Vector3 v)
        => new Vector3D(v.X, v.Y, v.Z);

    public static Point ToWpfPoint(this Vector2 v)
        => new Point(v.X, v.Y);

    public static Point3D ToWpfPoint(this Vector3 v)
        => new Point3D(v.X, v.Y, v.Z);

    public static Point3D ToWpfPoint(this DVector3 v)
        => new Point3D(v.X, v.Y, v.Z);

    public static Matrix4x4 ToAra3D(this Matrix3D t)
        => new Matrix4x4(
            (float)t.M11, (float)t.M12, (float)t.M13, (float)t.M14,
            (float)t.M21, (float)t.M22, (float)t.M23, (float)t.M24,
            (float)t.M31, (float)t.M32, (float)t.M33, (float)t.M34,
            (float)t.OffsetX, (float)t.OffsetY, (float)t.OffsetZ, (float)t.M44);

    public static Matrix3D ToWpf_Flipped(this Matrix4x4 t)
        => new Matrix3D(
            t.M11, t.M21, t.M31, t.M41,
            t.M12, t.M22, t.M32, t.M42,
            t.M13, t.M23, t.M33, t.M43,
            t.M14, t.M24, t.M34, t.M44);

    public static Matrix3D ToWpf(this Matrix4x4 t)
        => new Matrix3D(
            t.M11, t.M12, t.M13, t.M14,
            t.M21, t.M22, t.M23, t.M24,
            t.M31, t.M32, t.M33, t.M34,
            t.M41, t.M42, t.M43, t.M44);

    public static Transform3D ToWpfTransform(this Matrix4x4 t)
        => new MatrixTransform3D(ToWpf(t));

    public static Model3D ToWpfModel3D(this MeshGeometry3D mesh, Material material, Transform3D transform)
        => new GeometryModel3D(mesh, material) { Transform = transform };

    public static Model3D ToWpfModel3D(this MeshGeometry3D mesh, Material material, Matrix4x4 matrix)
        => new GeometryModel3D(mesh, material);// { Transform = matrix.ToWpfTransform() };

    public static Model3D ToWpfModel3D(this MeshGeometry3D mesh, Material material)
        => new GeometryModel3D(mesh, material);

    public static Point3DCollection ToPointCollection(this IArray<Vector3> xs)
        => new Point3DCollection(xs.ToEnumerable().Select(v => v.ToWpfPoint()));

    public static PointCollection ToPointCollection(this IArray<Vector2> xs)
        => new PointCollection(xs.ToEnumerable().Select(v => v.ToWpfPoint()));

    public static Vector3DCollection ToVectorCollection(this IArray<Vector3> xs)
        => new Vector3DCollection(xs.ToEnumerable().Select(v => v.ToWpf()));

    public static Int32Collection ToIntCollection(this IArray<int> xs)
        => new Int32Collection(xs.ToEnumerable());

    public static MeshGeometry3D ToMeshGeometry3D(this G3dMesh mesh)
    {
        var r = new MeshGeometry3D();
        if (mesh.VertexNormals?.Count > 0)
            r.Normals = ToVectorCollection(mesh.VertexNormals);
        r.Positions = mesh.Vertices.ToPointCollection();
        if (mesh.VertexUvs?.Count > 0)
            r.TextureCoordinates = mesh.VertexUvs.ToPointCollection();
        r.TriangleIndices = ToIntCollection(mesh.Indices);
        return r;
    }

    public static MeshGeometry3D ToMeshGeometry3D(this IMesh mesh)
        => new MeshGeometry3D
        {
            //if (mesh.VertexNormals?.Count > 0) r.Normals = ToVectorCollection(mesh.VertexNormals);
            //if (mesh.VertexUvs?.Count > 0) r.TextureCoordinates = mesh.VertexUvs.ToPointCollection();
            Positions = mesh.Vertices.ToPointCollection(),
            TriangleIndices = ToIntCollection(mesh.Indices())
        };
}