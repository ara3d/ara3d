using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara3D.Collections;
using Ara3D.Math;
using Ara3D.Serialization.G3D;
using Ara3D.Util;

namespace Ara3D.Geometry.ToRemove
{
    /*
    public static class GeometryAttributeExtensions
    {
    
        public static IMesh TriMesh(IEnumerable<GeometryAttribute> attributes)
            => attributes.Where(x => x != null).ToIMesh();

        public static IMesh TriMesh(params GeometryAttribute[] attributes)
            => TriMesh(attributes.AsEnumerable());

        public static IMesh TriMesh(
            this IArray<Vector3> vertices,
            IArray<int> indices = null,
            IArray<Vector2> uvs = null,
            IArray<Vector4> colors = null,
            IArray<int> materials = null,
            IArray<int> submeshMaterials = null)
            => TriMesh(
                vertices?.ToPositionAttribute(),
                indices?.ToIndexAttribute(),
                uvs?.ToVertexUvAttribute(),
                materials?.ToFaceMaterialAttribute(),
                colors?.ToVertexColorAttribute(),
                submeshMaterials?.ToSubmeshMaterialAttribute()
            );
        public static IMesh ToIMesh(this IArray<GeometryAttribute> self)
            => self.ToEnumerable().ToIMesh();

        public static IMesh ToIMesh(this IEnumerable<GeometryAttribute> self)
            => throw new NotImplementedException();

        public static IMesh ToIMesh(this IGeometryAttributes g)
            => throw new NotImplementedException();

        // NOTE: Material stuff really has more to do with graphics than geometry. 

        public static IArray<int> GetFaceMaterials(this IMesh mesh)
        {
            // SubmeshIndexOffsets: [0, A, B]
            // SubmeshIndexCount:   [X, Y, Z]
            // SubmeshMaterials:    [L, M, N]
            // ---
            // FaceMaterials:       [...Repeat(L, X / 3), ...Repeat(M, Y / 3), ...Repeat(N, Z / 3)] <-- divide by 3 for the number of corners per Triangular face
            var numCornersPerFace = mesh.NumCornersPerFace;
            return mesh.SubmeshIndexCount
                .ToEnumerable()
                .SelectMany((indexCount, i) =>
                    Enumerable.Repeat(mesh.SubmeshMaterials[i], indexCount / numCornersPerFace))
                .ToIArray();
        }

        public static IEnumerable<int> DisctinctMaterials(this IMesh mesh)
            => mesh.GetFaceMaterials().ToEnumerable().Distinct();

        public static DictionaryOfLists<int, int> IndicesByMaterial(this IMesh mesh)
        {
            var faceMaterials = mesh.GetFaceMaterials();
            return mesh.Indices.GroupBy(i => faceMaterials[i / 3]);
        }

        public static IMesh Merge(this IArray<IMesh> meshes)
            => meshes.Select(m => (IGeometryAttributes)m).Merge().ToIMesh();

        public static IEnumerable<(int Material, IMesh Mesh)> SplitByMaterial(this IMesh mesh)
        {
            var submeshMaterials = mesh.SubmeshMaterials;
            if (submeshMaterials == null || submeshMaterials.Count == 0)
            {
                // Base case: no submesh materials are defined on the mesh.
                return new[] { (-1, mesh) };
            }

            var submeshIndexOffets = mesh.SubmeshIndexOffsets;
            var submeshIndexCounts = mesh.SubmeshIndexCount;
            if (submeshIndexOffets == null || submeshIndexCounts == null ||
                submeshMaterials.Count <= 1 || submeshIndexOffets.Count <= 1 || submeshIndexCounts.Count <= 1)
            {
                // Base case: only one submesh material.
                return new[] { (submeshMaterials[0], mesh) };
            }

            // Example:
            //
            // ------------
            // INPUT MESH:
            // ------------
            // Vertices            [Va, Vb, Vc, Vd, Ve, Vf, Vg] <-- 7 vertices
            // Indices             [0 (Va), 1 (Vb), 2 (Vc), 1 (Vb), 2 (Vc), 3 (Vd), 4 (Ve), 5 (Vf), 6 (Vg)] <-- 3 triangles referencing the 7 vertices
            // SubmeshIndexOffsets [0, 3, 6]
            // SubmeshIndexCount   [3, 3, 3] (computed)
            // SubmeshMaterials    [Ma, Mb, Mc]
            //
            // ------------
            // OUTPUT MESHES
            // ------------
            // - MESH FOR MATERIAL Ma
            //   Vertices:             [Va, Vb, Vc]
            //   Indices:              [0, 1, 2]
            //   SubmeshIndexOffsets:  [0]
            //   SubmeshMaterials:     [Ma]
            //
            //- MESH FOR MATERIAL Mb
            //   Vertices:             [Vb, Vc, Vd]
            //   Indices:              [0, 1, 2]
            //   SubmeshIndexOffsets:  [0]
            //   SubmeshMaterials:     [Mb]
            //
            //- MESH FOR MATERIAL Mc
            //   Vertices:             [Ve, Vf, Vg]
            //   Indices:              [0, 1, 2]
            //   SubmeshIndexOffsets:  [0]
            //   SubmeshMaterials:     [Mc]

            return mesh.SubmeshMaterials
                .Select((submeshMaterial, submeshIndex) => (submeshMaterial, submeshIndex))
                .GroupBy(t => t.submeshMaterial)
                .SelectMany(g =>
                {
                    var material = g.Key;
                    var meshes = g.Select((t, _) =>
                    {
                        var submeshMaterial = t.submeshMaterial;
                        var submeshStartIndex = submeshIndexOffets[t.submeshIndex];
                        var submeshIndexCount = submeshIndexCounts[t.submeshIndex];

                        var indexSlice = mesh.Indices.Slice(submeshStartIndex, submeshStartIndex + submeshIndexCount);

                        var newVertexAttributes = mesh.VertexAttributes().Select(attr => attr.Remap(indexSlice));
                        var newIndexAttribute = indexSlice.Count.Select(i => i).ToIndexAttribute();

                        var newSubmeshIndexOffsets = 0.Repeat(1).ToSubmeshIndexOffsetAttribute();
                        var newSubmeshMaterials = submeshMaterial.Repeat(1).ToSubmeshMaterialAttribute();

                        return newVertexAttributes
                            .Concat(mesh.NoneAttributes())
                            .Concat(mesh.WholeGeometryAttributes())
                            // TODO: TECH DEBT - face, edge, and corner attributes are ignored for now.
                            .Append(newIndexAttribute)
                            .Append(newSubmeshIndexOffsets)
                            .Append(newSubmeshMaterials)
                            .ToGeometryAttributes()
                            .ToIMesh();
                    });

                    return meshes.Select(m => (material, m));
                });
        }

        public static IGeometryAttributes DeleteUnusedVertices(this IMesh mesh)
        {
            var tmp = new bool[mesh.Vertices.Count];
            for (var i = 0; i < mesh.Indices.Count; ++i)
                tmp[mesh.Indices[i]] = true;

            var remap = new List<int>();
            for (var i = 0; i < tmp.Length; ++i)
            {
                if (tmp[i])
                    remap.Add(i);
            }

            return mesh.RemapVertices(remap.ToIArray());
        }

        public static IMesh Merge(this IEnumerable<IMesh> meshes)
            => meshes.ToIArray().Merge();

        public static IGeometryAttributes ReverseWindingOrder(this IMesh mesh)
        {
            var n = mesh.Indices.Count;
            var r = new int[n];
            for (var i = 0; i < n; i += 3)
            {
                r[i + 0] = mesh.Indices[i + 2];
                r[i + 1] = mesh.Indices[i + 1];
                r[i + 2] = mesh.Indices[i + 0];
            }

            return mesh.SetAttribute(r.ToIArray().ToIndexAttribute());
        }


        public static IArray<T> FaceDataToCornerData<T>(this IMesh mesh, IArray<T> data)
            => mesh.GetNumCorners.Select(i => data[i / 3]);

        public static IArray<Vector3> GetOrComputeFaceNormals(this IMesh mesh)
            => mesh.GetAttributeFaceNormal()?.Data ?? mesh.ComputedNormals();

        public static IArray<Vector3> GetOrComputeVertexNormals(this IMesh mesh)
            => mesh.VertexNormals ?? mesh.ComputeTopology().GetOrComputeVertexNormals();


        /// <summary>
        /// Returns vertex normals if present, otherwise computes vertex normals naively by averaging them.
        /// Given a pre-computed topology, will-leverage that.
        /// A more sophisticated algorithm would compute the weighted normal 
        /// based on an angle.
        /// </summary>
        public static IArray<Vector3> GetOrComputeVertexNormals(this Topology topo)
        {
            var mesh = topo.Mesh;
            var r = mesh.VertexNormals;
            if (r != null) return r;
            var faceNormals = mesh.GetOrComputeFaceNormals().ToArray();
            return mesh
                .NumVertices
                .Select(vi =>
                {
                    var tmp = topo
                        .FacesFromVertexIndex(vi)
                        .Select(fi => faceNormals[fi])
                        .Average();
                    if (tmp.IsNaN())
                        return Vector3.Zero;
                    return tmp.SafeNormalize();
                });
        }



        public static IMesh CopyFaces(this IMesh mesh, Func<int, bool> predicate)
            => (mesh as IGeometryAttributes).CopyFaces(predicate).ToIMesh();

        public static IMesh CopyFaces(this IMesh mesh, IArray<bool> keep)
            => CopyFaces(mesh, i => keep[i]);

        public static IMesh CopyFaces(this IMesh mesh, IArray<int> keep)
            => mesh.RemapFaces(keep).ToIMesh();

        public static IMesh DeleteFaces(this IMesh mesh, Func<int, bool> predicate)
            => mesh.CopyFaces(f => !predicate(f));

        /// <summary>
        /// Given an array of data associated with corners, return an array of data associated with
        /// vertices. If a vertex is not referenced, no data is returned. If a vertex is referenced
        /// multiple times, the last reference is used.
        /// TODO: supplement with a proper interpolation system.
        /// </summary>
        public static IArray<T> CornerDataToVertexData<T>(this IMesh mesh, IArray<T> data)
        {
            var vertexData = new T[mesh.NumVertices];
            for (var i = 0; i < data.Count; ++i)
                vertexData[mesh.Indices[i]] = data[i];
            return vertexData.ToIArray();
        }


        /// <summary>
        /// Given an array of face data, creates an array of indexed data to match vertices
        /// </summary>
        public static IArray<T> FaceDataToVertexData<T>(this IMesh mesh, IArray<T> data)
        {
            if (data.Count != mesh.GetNumFaces())
                throw new Exception("Cannot match input Face data to existing faces");

            var vertexData = new T[mesh.GetNumVertices()];
            for (var i = 0; i < mesh.GetNumCorners(); ++i)
                vertexData[mesh.Indices[i]] = data[i / 3];
            return vertexData.ToIArray();
        }


        public static IMesh Merge(this IMesh mesh, params IMesh[] others)
        {
            var gs = others.ToList();
            gs.Insert(0, mesh);
            return gs.Merge();
        }
    }
    */
}
