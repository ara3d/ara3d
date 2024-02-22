using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Math;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Transform = Ara3D.Math.Transform;

namespace Ara3D.Interop.Revit
{
    public static class RevitInterop
    {
        public static DVector3 ToAra(this XYZ v)
            => (v.X, v.Y, v.Z);

        public static Int3 ToAra(this MeshTriangle tri)
            => ((int)tri.get_Index(0), 
                (int)tri.get_Index(1), 
                (int)tri.get_Index(2));

        public static IMesh ToAra(this Mesh mesh)
            => new TriMesh(
                mesh.Vertices.Select(v => v.ToAra().Vector3).ToIArray(),
                mesh.NumTriangles.Select(i => mesh.get_Triangle(i).ToAra()));

        public static Vector2 ToVector2(this UV p)
            => p.ToDVector2().Vector2;

        public static Vector3 ToVector3(this XYZ xyz)
            => xyz.ToDVector3().Vector3;

        public static DVector2 ToDVector2(this UV uv)
            => new DVector2(uv.U, uv.V);

        public static DVector3 ToDVector3(this XYZ xyz)
            => new DVector3(xyz.X, xyz.Y, xyz.Z);

        public static DAABox ToDAABox(this Outline outline)
            => outline == null
                ? new DAABox()
                : new DAABox(outline.MinimumPoint.ToDVector3(), outline.MaximumPoint.ToDVector3());

        public static DAABox ToDAABox(this BoundingBoxXYZ box)
            => box == null
                ? new DAABox()
                : new DAABox(box.Min.ToDVector3(), box.Max.ToDVector3());

        public static DAABox2D ToDAABox2D(this BoundingBoxUV box)
            => box == null
                ? new DAABox2D()
                : new DAABox2D(box.Min.ToDVector2(), box.Max.ToDVector2());

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
