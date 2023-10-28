using UnityEngine;
using System.Linq;
using System;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Math;
using Matrix4x4 = Ara3D.Math.Matrix4x4;
using Vector2 = Ara3D.Math.Vector2;
using Vector3 = Ara3D.Math.Vector3;

namespace Ara3D.UnityBridge
{
    /// <summary>
    /// A copy of the Topology, UVs, Vertices, Colors, and Normals, of a Unity mesh.
    /// This is done in a form that is serializable
    /// TODO: finish the implementation.
    /// TODO: decide whether this should be immutable or not.
    /// TODO: decide whether this should implement IRenderMesh
    /// TODO: consider uvs1 through 8, tangents, and boneWeights
    /// https://docs.unity3d.com/ScriptReference/BoneWeight.html
    /// </summary>
    public class UnityMesh : IMesh
    {
        public UnityEngine.Vector2[] UnityUVs;
        public UnityEngine.Vector3[] UnityVertices;
        public UnityEngine.Vector3[] UnityNormals;

        public int[] UnityIndices;
        public Color32[] UnityColors;

        public IArray<Int3> Faces => UnityIndices.ToIArray().SelectTriplets((a, b, c) => new Int3(a, b, c));
        public IArray<Vector3> Vertices => UnityVertices.ToIArray().Select(v => v.ToAra3D());
        public IArray<Vector2> UVs => UnityUVs.ToIArray().Select(v => v.ToAra3D());
        public IArray<Vector3> Normals => UnityNormals.ToIArray().Select(v => v.ToAra3D());

        // TODO: this needs to be improved (assumes TriMesh, etc.)
        public void FromAra3D(IMesh g)
        {
            UnityIndices = g.Indices().ToArray();
            UnityVertices = g.Vertices.Select(UnityConverters.ToUnity).ToArray();
        }

        public UnityMesh(UnityMesh other)
        {
            CopyFrom(other);
        }

        public UnityMesh Clone()
        {
            return new UnityMesh(this);
        }

        public void CopyFrom(UnityMesh other)
        {
            UnityIndices = other.UnityIndices;
            UnityUVs = other.UnityUVs?.ToArray();
            UnityVertices = other.UnityVertices?.ToArray();
            UnityColors = other.UnityColors?.ToArray();
            UnityNormals = other.UnityNormals?.ToArray();
        }

        public void CopyFrom(Mesh mesh)
        {
            UnityIndices = mesh.triangles;
            UnityUVs = mesh.uv?.ToArray();
            UnityVertices = mesh.vertices?.ToArray();
            UnityColors = mesh.colors32?.ToArray();
            UnityNormals = mesh.normals?.ToArray();
        }

        public UnityMesh(Mesh mesh)
        {
            CopyFrom(mesh);
        }

        public void AssignToMesh(Mesh mesh)
        {
            mesh.Clear();
            if (UnityVertices != null) mesh.vertices = UnityVertices;
            if (UnityIndices != null) mesh.triangles = UnityIndices;
            if (UnityUVs != null) mesh.uv = UnityUVs;
            if (UnityColors != null) mesh.colors32 = UnityColors;
            if (UnityNormals != null) mesh.normals = UnityNormals;
        }

        IGeometry ITransformable<IGeometry>.Transform(Matrix4x4 mat)
        {
            throw new NotImplementedException();
        }

        IGeometry IDeformable<IGeometry>.Deform(Func<Vector3, Vector3> f)
        {
            throw new NotImplementedException();
        }

        IMesh ITransformable<IMesh>.Transform(Matrix4x4 mat)
        {
            throw new NotImplementedException();
        }

        IMesh IDeformable<IMesh>.Deform(Func<Vector3, Vector3> f)
        {
            throw new NotImplementedException();
        }
    }
}
