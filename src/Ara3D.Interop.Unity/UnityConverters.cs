using System;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;
using Ara3D.Serialization.G3D;
using Ara3D.Serialization.VIM;
using UnityEngine;
using UnityEngine.Rendering;
using Matrix4x4 = Ara3D.Mathematics.Matrix4x4;
using Mesh = UnityEngine.Mesh;
using MeshTopology = UnityEngine.MeshTopology;
using Quaternion = Ara3D.Mathematics.Quaternion;
using Transform = UnityEngine.Transform;
// Explicitly specify math types to make it clear what each function does
using UVector4 = UnityEngine.Vector4;
using UVector3 = UnityEngine.Vector3;
using UVector2 = UnityEngine.Vector2;
using UQuaternion = UnityEngine.Quaternion;
using Vector2 = Ara3D.Mathematics.Vector2;
using Vector3 = Ara3D.Mathematics.Vector3;
using Vector4 = Ara3D.Mathematics.Vector4;

namespace Ara3D.UnityBridge
{
    public static class UnityConverters
    {
        // When translating G3D faces to unity we need
        // to reverse the triangle winding.
        public static int PolyFaceToUnity(int index, int faceSize)
        {
            var faceIdx = index / faceSize;
            var vertIdx = index % faceSize;
            return (vertIdx == 0) ? index : (faceIdx * faceSize) + (faceSize - vertIdx);
        }
            
        // Remaps 1, 2, 3 to 1, 3, 2
        public static int TriFaceToUnity(int index)
            => PolyFaceToUnity(index, 3);

        public static int QuadFaceToUnity(int index)
            => PolyFaceToUnity(index, 4);

        // TODO: This should be pushed into the exporter.  The world deals in metric.
        public static UVector3 ToUnity(this Vector3 v)
            => new UVector3(v.X, v.Y, v.Z);

        public static UVector2 ToUnity(this Vector2 v)
            => new UVector2(v.X, v.Y);

        public static UVector3 PositionToUnity(float x, float y, float z)
            => new UVector3(-x, z, -y);

        public static UVector3 PositionToUnity(Vector3 pos)
            => PositionToUnity(pos.X, pos.Y, pos.Z);

        public static UQuaternion RotationToUnity(Quaternion rot)
            => new UQuaternion(rot.X, -rot.Z, rot.Y, rot.W);

        public static UVector3 SwizzleToUnity(float x, float y, float z)
            => new UVector3(x, z, y);

        public static UVector3 SwizzleToUnity(Vector3 v)
            => SwizzleToUnity(v.X, v.Z, v.Y);

        public static UVector3 ScaleToUnity(Vector3 scl)
            => SwizzleToUnity(scl);

        public static Bounds ToUnity(this AABox box)
            => new Bounds(PositionToUnity(box.Center), SwizzleToUnity(box.Extent));

        public static UVector3[] ToUnity(this IArray<Vector3> vertices)
            => vertices.Select(ToUnity).ToArray();

        public static Mesh UpdateMeshVertices(this Mesh mesh, IArray<Vector3> vertices)
        {
            mesh.vertices = vertices.ToUnity();
            return mesh;
        }

        public static int[] ToUnityIndexBuffer(this IArray<int> indices)
            => indices.ReverseTriangleIndexOrder().ToArray();

        // 0, 1, 2, 3, 4, 5 ... => 2, 1, 0, 5, 4, 3 ...
        public static IArray<int> ReverseTriangleIndexOrder(this IArray<int> indices)
            => indices.SelectByIndex(indices.Count.Select(i => ((i / 3) + 1) * 3 - 1 - (i % 3)));

        public static MeshTopology FaceSizeToMeshTopology(int faceSize)
        {
            switch (faceSize)
            {
                case 1: return MeshTopology.Points;
                case 2: return MeshTopology.Lines;
                case 3: return MeshTopology.Triangles;
                case 4: return MeshTopology.Quads;
            }
            throw new Exception("Unsupported mesh topology");
        }

        public static Mesh UpdateMeshIndices(this Mesh mesh, IArray<int> indices, int faceSize)
        {
            switch (faceSize)
            {
                case 1: return mesh.UpdateMeshPoints(indices);
                case 2: return mesh.UpdateMeshLines(indices);
                case 3: return mesh.UpdateMeshTriangleIndices(indices);
                case 4: return mesh.UpdateMeshQuadIndices(indices);
            }
            throw new Exception("Only face sizes of 1 to 4 are supported");
        }

        public static Mesh UpdateMeshTriangleIndices(this Mesh mesh, IArray<int> triangleIndices)
        {
            if (triangleIndices.Count % 3 != 0)
                throw new Exception("Triangle index buffer must have a count divisible by 3");
            mesh.SetIndices(triangleIndices.MapIndices(TriFaceToUnity).ToArray(), MeshTopology.Triangles, 0);
            return mesh;
        }

        public static Mesh UpdateMeshQuadIndices(this Mesh mesh, IArray<int> quadIndices)
        {
            if (quadIndices.Count % 4 != 0)
                throw new Exception("Quad index buffer must have a count divisible by 4");
            mesh.SetIndices(quadIndices.MapIndices(QuadFaceToUnity).ToArray(), MeshTopology.Quads, 0);
            return mesh;
        }

        public static Mesh UpdateMeshLines(this Mesh mesh, IArray<int> lineIndices)
        {
            if (lineIndices.Count % 2 != 0)
                throw new Exception("Line index buffer must have a count divisible by 2");
            mesh.SetIndices(lineIndices.ToArray(), MeshTopology.Lines, 0);
            return mesh;
        }

        public static Mesh UpdateMeshPoints(this Mesh mesh, IArray<int> pointIndices)
        {
            mesh.SetIndices(pointIndices.ToArray(), MeshTopology.Points, 0);
            return mesh;
        }

        public static Mesh UpdateMesh(this Mesh mesh, IArray<Vector3> vertices, IArray<int> indices, int pointsPerFace)
        {
            mesh.Clear(false);
            mesh.indexFormat = vertices.Count > ushort.MaxValue
                ? IndexFormat.UInt32
                : IndexFormat.UInt16;
            mesh.UpdateMeshVertices(vertices);
            mesh.UpdateMeshIndices(indices, pointsPerFace);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh UpdateMesh(this Mesh mesh, ITriMesh g)
        {
            if (mesh == null || g == null)
                return mesh;

            return mesh.UpdateMesh(g.Points, g.Indices(), 3);
            
            // TODO: copy colors, normals, uvs1 through 8, tangents, and boneWeights
            //r.colors = g.VertexColors.Select();
            //r.normals = g.VertexNormals.Select();
            // r.uv(8) = g.UV.Select();
            //r.tangents;
            //https://docs.unity3d.com/ScriptReference/BoneWeight.html
            // r.boneWeights
            //return r;
        }

        public static Mesh GetMesh(this MeshFilter filter)
            => filter == null ? null : filter.sharedMesh;

        public static Mesh GetMesh(this GameObject obj)
            => obj == null ? null : obj.GetComponent<MeshFilter>().GetMesh();

        public static Mesh UpdateMesh(this GameObject obj, ITriMesh m)
            => obj == null ? null : UpdateMesh(obj.GetMesh(), m);

        public static Mesh CreateMesh(this MonoBehaviour mono, bool renderable = true)
        {
            if (mono == null || mono.gameObject == null)
                return null;
            if (renderable)
            {
                if (mono.gameObject.GetComponent<MeshRenderer>() == null)
                    mono.gameObject.AddComponent<MeshRenderer>();
            }
            var filter = mono.gameObject.GetComponent<MeshFilter>();
            if (filter == null)
                return mono.gameObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();
            return filter.sharedMesh;
        }

        public static Mesh GetMesh(this MonoBehaviour mono)
            => mono.GetComponent<MeshFilter>().GetMesh();

        public static Mesh UpdateMesh(this MonoBehaviour mono, ITriMesh m)
            => UpdateMesh(mono.GetMesh(), m);

        public static Mesh ToUnity(this ITriMesh m)
            => UpdateMesh(new Mesh(), m);

        public static void SetFromMatrix(this Transform transform, Matrix4x4 matrix) {
            var decomposed = Matrix4x4.Decompose(matrix, out var scl, out var rot, out var pos);
            if (!decomposed)
                throw new Exception("Can't decompose matrix");
            transform.position = PositionToUnity(pos);
            transform.rotation = RotationToUnity(rot);
            transform.localScale = ScaleToUnity(scl);
        }

        public static (UVector3 pos, UQuaternion rot, UVector3 scl) ToUnityTRS(this Matrix4x4 matrix)
        {
            var decomposed = Matrix4x4.Decompose(matrix, out var scl, out var rot, out var pos);
            if (!decomposed)
                throw new Exception("Can't decompose matrix");

            return ToUnityTRS(pos, rot, scl);
        }

        public static UnityEngine.Matrix4x4 ToUnityFlipped(this Matrix4x4 matrix)
        {
            var decomposed = Matrix4x4.Decompose(matrix, out var scl, out var rot, out var pos);
            if (!decomposed)
                throw new Exception("Can't decompose matrix");

            var (t, r, s) = ToUnityTRS(pos, rot, scl);
            return UnityEngine.Matrix4x4.TRS(t, r, s);
        }

        /// <summary>
        /// Converts the given VIM based coordinates into Unity coordinates.
        /// </summary>
        public static (UVector3 pos, UQuaternion rot, UVector3 scl) ToUnityTRS(
            Vector3 pos,
            Quaternion rot,
            Vector3 scale
        )
        {
            // Pose space is mirrored on X, and then rotated 90 degrees around X
            var p = PositionToUnity(pos);

            // Quaternion is mirrored the same way, but then negated via W = -W because that's just easier to read
            var r = RotationToUnity(rot);

            // TODO: test this, current scale is completely untested
            var s = ScaleToUnity(scale);

            return (p, r, s);
        }

        public static UnityEngine.Matrix4x4 ToUnityRaw(this Matrix4x4 matrix)
            => new UnityEngine.Matrix4x4(
                new UVector4(matrix.M11, matrix.M12, matrix.M13, matrix.M14),
                new UVector4(matrix.M21, matrix.M22, matrix.M23, matrix.M24),
                new UVector4(matrix.M31, matrix.M32, matrix.M33, matrix.M34),
                new UVector4(matrix.M41, matrix.M42, matrix.M43, matrix.M44)
            );

        private const float ftm = 0.3408f;
        public static UnityEngine.Matrix4x4 ConversionMatrix = new UnityEngine.Matrix4x4(
            new UVector4(-ftm, 0, 0, 0),
            new UVector4(0, 0, -ftm, 0),
            new UVector4(0, ftm, 0, 0),
            new UVector4(0, 0, 0, 1)
        );

        public static UnityEngine.Matrix4x4 ToUnity(this Matrix4x4 matrix)
            => ConversionMatrix * ToUnityRaw(matrix);

        public static Vector2 ToAra3D(this UVector2 v)
            => new Vector2(v.x, v.y);

        public static Vector3 ToAra3D(this UVector3 v)
            => new Vector3(v.x, v.y, v.z);

        public static Vector4 ToAra3D(this UVector4 v)
            => new Vector4(v.x, v.y, v.z, v.w);

        public static Quaternion ToAra3D(this UQuaternion q)
            => new Quaternion(q.x, q.y, q.z, q.w);

        public static IArray<Vector2> ToAra3D(this UVector2[] xs)
            => xs.ToIArray().Select(x => x.ToAra3D());

        public static IArray<Vector3> ToAra3D(this UVector3[] xs)
            => xs.ToIArray().Select(x => x.ToAra3D());

        public static IArray<Vector4> ToAra3D(this UVector4[] xs)
            => xs.ToIArray().Select(x => x.ToAra3D());

        public static IArray<Quaternion> ToAra3D(this UQuaternion[] xs)
            => xs.ToIArray().Select(x => x.ToAra3D());

        public static UVector4 ToUnity(this Vector4 v)
            => new UVector4(v.X, v.Y, v.Z, v.W);

        public static Color ToUnityColor(this Vector4 v)
            => v.ToUnity();

        public static UnityTriMesh ToUnity(this G3dMesh mesh)
        {
            return new UnityTriMesh()
            {
                UnityIndices = mesh.Indices.ToArray(),
                UnityVertices = mesh.Vertices.Select(ToUnity).ToArray(),
                // TODO: normals and UVs
            };
        }

        public static UnityMeshScene ToUnity(this SerializableDocument doc)
        {
            var g = doc.Geometry;
            var r = new UnityMeshScene();
            
            var defaultColor = new Color(0.6f, 0.6f, 0.75f, 1f);
            for (var i =0; i < g.Meshes.Count; i++)
            {
                var m = g.Meshes[i];

                var matIndex = m.Submeshes.Count > 0 
                    ? m.Submeshes[0].MaterialIndex 
                    : -1;
                
                var set = new UnityMeshInstanceSet
                {
                    TriMesh = m.ToUnity(),
                    Color = matIndex >= 0
                        ? g.MaterialColors[matIndex].ToUnityColor()
                        : defaultColor
                };
                r.InstanceSets.Add(set);
            }

            for (var i = 0; i < g.InstanceTransforms.Count; i++)
            {
                var t = g.InstanceTransforms[i];
                var idx = g.InstanceMeshes[i];
                if (idx < 0) continue;
                if (idx > r.InstanceSets.Count) continue;
                var set = r.InstanceSets[idx];
                set.Matrices.Add(t.ToUnity());
            }

            return r;
        }
    }
}
