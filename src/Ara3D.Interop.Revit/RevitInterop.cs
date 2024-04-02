using System.Linq;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;
using Autodesk.Revit.DB;

namespace Ara3D.Interop.Revit
{
    public static class RevitInterop
    {
        public static Vector3 ToAra(this XYZ v)
            => ((float)v.X, (float)v.Y, (float)v.Z);

        public static Int3 ToAra(this MeshTriangle tri)
            => ((int)tri.get_Index(0), 
                (int)tri.get_Index(1), 
                (int)tri.get_Index(2));

        public static ITriMesh ToAra(this Autodesk.Revit.DB.Mesh mesh)
            => new TriMesh(
                mesh.Vertices.Select(v => v.ToAra()).ToIArray(),
                mesh.NumTriangles.Select(i => mesh.get_Triangle(i).ToAra()));

        
        public static Vector2 ToVector2(this UV uv)
            => ((float)uv.U, (float)uv.V);

        public static Vector3 ToVector3(this XYZ xyz)
            => ((float)xyz.X, (float)xyz.Y, (float)xyz.Z);

        public static AABox ToAABox(this Outline outline)
            => outline == null
                ? new AABox()
                : new AABox(outline.MinimumPoint.ToVector3(), outline.MaximumPoint.ToVector3());

        public static AABox ToAABox(this BoundingBoxXYZ box)
            => box == null
                ? new AABox()
                : new AABox(box.Min.ToVector3(), box.Max.ToVector3());

        public static AABox2D ToAABox2D(this BoundingBoxUV box)
            => box == null
                ? new AABox2D()
                : new AABox2D(box.Min.ToVector2(), box.Max.ToVector2());

        // TODO: convert to DMatrix4x4
        public static Matrix4x4 ToMatrix4x4(this Autodesk.Revit.DB.Transform t)
        {
            if (t == null || t.IsIdentity)
                return Matrix4x4.Identity;

            var x = t.BasisX.ToVector3();
            var y = t.BasisY.ToVector3();
            var z = t.BasisZ.ToVector3();
            var tr = t.Origin.ToVector3();

            return new Matrix4x4(
                x.X, x.Y, x.Z, 0,
                y.X, y.Y, y.Z, 0,
                z.X, z.Y, z.Z, 0,
                tr.X, tr.Y, tr.Z, 1);
        }
    }
}
