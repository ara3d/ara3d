using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Serialization.BFAST;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.Serialization.G3D
{
    /// <summary>
    /// Represents a basic single-precision G3D in memory, with access to common attributes.
    /// The G3D format can be double precision, but this data structure won't provide access to all of the attributes.
    /// In the case of G3D formats that are non-conformant to the expected semantics you can use
    /// GeometryAttributes.
    /// This class is inspired heavily by the structure of FBX and Assimp.
    /// Potentially huge arrays are left as "IArray" to avoid copying, while smaller arrays
    /// are computed as literal arrays and stored in memory using IReadOnlyList references. 
    /// </summary>
    public class G3D : GeometryAttributes
    {
        public new static readonly G3D Empty = Create();

        public G3dHeader Header { get; }

        /// <summary>
        /// Vertex buffer. Required to be present. 
        /// </summary>
        public IArray<Vector3> Vertices { get; }

        /// <summary>
        /// Index buffer (one index per corner, and per half-edge). Computed if absent. 
        /// </summary>
        public IArray<int> Indices { get; }

        /// <summary>
        /// Arbitrary number of UV channels. Ordered as they appear in the file. 
        /// </summary>
        public List<IArray<Vector2>> AllVertexUvs { get; } = new List<IArray<Vector2>>();

        /// <summary>
        /// Arbitrary number of vertex color channels. Ordered as they appear in the file.
        /// </summary>
        public List<IArray<Vector4>> AllVertexColors { get; } = new List<IArray<Vector4>>();

        /// <summary>
        /// The first UV channel is the default UV channel.
        /// </summary>
        public IArray<Vector2> VertexUvs => AllVertexUvs?.ElementAtOrDefault(0);

        /// <summary>
        /// The default vertex color channel is the first one. 
        /// </summary>
        public IArray<Vector4> VertexColors => AllVertexColors?.ElementAtOrDefault(0);
        
        /// <summary>
        /// Vertex normals channel. 
        /// </summary>
        public IArray<Vector3> VertexNormals { get; }

        /// <summary>
        /// Vertex tangents channel. 
        /// </summary>
        public IArray<Vector4> VertexTangents { get; }

        /// <summary>
        /// Material indices per face. 
        /// </summary>
        public IArray<int> FaceMaterials { get; }

        /// <summary>
        /// The normal of each face, if not provided, are computed dynamically as the average of all vertex normals,
        /// </summary>
        public IArray<Vector3> FaceNormals { get; } 

        /// <summary>
        /// Offset into the index buffer for each Mesh.
        /// </summary>
        public IReadOnlyList<int> MeshIndexOffsets { get; }

        /// <summary>
        /// Number of indices for each Mesh. Computed.  
        /// </summary>
        public IReadOnlyList<int> MeshIndexCounts { get; }

        /// <summary>
        /// For each mesh, this is the index of the first sub-mesh that it is associated with.
        /// </summary>
        public IReadOnlyList<int> MeshSubmeshOffset { get; }

        /// <summary>
        /// For each mesh, this is the number of sub-meshes that it is associated with. Computed.  
        /// </summary>
        public IReadOnlyList<int> MeshSubmeshCount { get; } 

        /// <summary>
        /// Computed array of mesh data structures for convenience. 
        /// </summary>
        public IReadOnlyList<G3dMesh> Meshes { get; }

        /// <summary>
        /// Index of the parent transforms. If present. Usually not used. 
        /// </summary>
        public IReadOnlyList<int> InstanceParents { get; }

        /// <summary>
        /// // A 4x4 matrix in row-column order defining the transformed mesh.
        /// </summary>
        public IReadOnlyList<Matrix4x4> InstanceTransforms { get; }

        /// <summary>
        /// The index of the mesh associated with the instance transform.
        /// </summary>
        public IReadOnlyList<int> InstanceMeshes { get; }

        /// <summary>
        /// Custom data (for example visibility flags) associated with each instance.
        /// </summary>
        public IReadOnlyList<ushort> InstanceFlags { get; }

        /// <summary>
        /// Material colors as RGBA
        /// </summary>
        public IReadOnlyList<Vector4> MaterialColors { get; } 

        /// <summary>
        /// Material glossiness 
        /// </summary>
        public IReadOnlyList<float> MaterialGlossiness { get; }
        
        /// <summary>
        /// Material smoothness 
        /// </summary>
        public IReadOnlyList<float> MaterialSmoothness { get; }
        
        /// <summary>
        /// Material structures. 
        /// </summary>
        public IReadOnlyList<G3dMaterial> Materials { get; }

        /// <summary>
        /// The offset into the index buffer for each submesh.
        /// Used to compute the offset into the index buffer for a corresponding mesh as well. 
        /// </summary>
        public IReadOnlyList<int> SubmeshIndexOffsets { get; }

        /// <summary>
        /// The number of indices associated with a submesh.
        /// Usually computed. 
        /// </summary>
        public IReadOnlyList<int> SubmeshIndexCount { get; }
        
        /// <summary>
        /// The index of the material associated with a submesh. 
        /// </summary>
        public IReadOnlyList<int> SubmeshMaterials { get; }

        /// <summary>
        /// Constructor 
        /// </summary>
        public G3D(IEnumerable<GeometryAttribute> attributes, G3dHeader? header = null, int numCornersPerFaceOverride = -1)
            : base(attributes, numCornersPerFaceOverride)
        {
            Header = header ?? new G3dHeader();

            foreach (var attr in Attributes.ToEnumerable())
            {
                var desc = attr.Descriptor;
                switch (desc.Semantic)
                {
                    case Semantic.Index:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_corner))
                            Indices = Indices ?? attr.AsType<int>().Data;
                        if (attr.IsTypeAndAssociation<short>(Association.assoc_corner))
                            Indices = Indices ?? attr.AsType<short>().Data.Select(x => (int)x);
                        break;

                    case Semantic.Position:
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_vertex))
                            Vertices = Vertices ?? attr.AsType<Vector3>().Data;
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_corner))
                            Vertices = Vertices ?? attr.AsType<Vector3>().Data; // TODO: is this used?
                        break;

                    case Semantic.Tangent:
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_vertex))
                            VertexTangents = VertexTangents ?? attr.AsType<Vector3>().Data.Select(v => v.ToVector4());
                        if (attr.IsTypeAndAssociation<Vector4>(Association.assoc_vertex))
                            VertexTangents = VertexTangents ?? attr.AsType<Vector4>().Data;
                        break;

                    case Semantic.Uv:
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_vertex))
                            AllVertexUvs.Add(attr.AsType<Vector3>().Data.Select(uv => uv.ToVector2()));
                        if (attr.IsTypeAndAssociation<Vector2>(Association.assoc_vertex))
                            AllVertexUvs.Add(attr.AsType<Vector2>().Data);
                        break;

                    case Semantic.Color:
                        if (desc.Association == Association.assoc_vertex)
                            AllVertexColors.Add(attr.AttributeToColors());
                        if (desc.Association == Association.assoc_material)
                            MaterialColors = MaterialColors ?? attr.AttributeToColors().ToArray();
                        break;

                    case Semantic.IndexOffset:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_mesh))
                            MeshIndexOffsets = MeshIndexOffsets ?? attr.AsType<int>().Data.ToArray();
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_submesh))
                            SubmeshIndexOffsets = SubmeshIndexOffsets ?? attr.AsType<int>().Data.ToArray();
                        break;

                    case Semantic.Normal:
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_face))
                            FaceNormals = FaceNormals ?? attr.AsType<Vector3>().Data;
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_vertex))
                            VertexNormals = VertexNormals ?? attr.AsType<Vector3>().Data;
                        break;

                    case Semantic.Material:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_face))
                            FaceMaterials = FaceMaterials ?? attr.AsType<int>().Data;
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_submesh))
                            SubmeshMaterials = SubmeshMaterials ?? attr.AsType<int>().Data.ToArray();
                        break;

                    case Semantic.Parent:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_instance))
                            InstanceParents = InstanceParents ?? attr.AsType<int>().Data.ToArray();
                        break;

                    case Semantic.Mesh:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_instance))
                            InstanceMeshes = InstanceMeshes ?? attr.AsType<int>().Data.ToArray();
                        break;

                    case Semantic.Transform:
                        if (attr.IsTypeAndAssociation<Matrix4x4>(Association.assoc_instance))
                            InstanceTransforms = InstanceTransforms ?? attr.AsType<Matrix4x4>().Data.ToArray();
                        break;

                    case Semantic.Glossiness:
                        if (attr.IsTypeAndAssociation<float>(Association.assoc_material))
                            MaterialGlossiness = attr.AsType<float>().Data.ToArray();
                        break;

                    case Semantic.Smoothness:
                        if (attr.IsTypeAndAssociation<float>(Association.assoc_material))
                            MaterialSmoothness = attr.AsType<float>().Data.ToArray();
                        break;

                    case Semantic.SubMeshOffset:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_mesh))
                            MeshSubmeshOffset = attr.AsType<int>().Data.ToArray();
                        break;

                    case Semantic.Flags:
                        if (attr.IsTypeAndAssociation<ushort>(Association.assoc_instance))
                            InstanceFlags = attr.AsType<ushort>().Data.ToArray();
                        break;
                }
            }

            // If no vertices are provided, we are going to generate a list of zero vertices.
            if (Vertices == null)
                Vertices = Vector3.Zero.Repeat(0);

            // If no indices are provided then we are going to have to treat the index buffer as indices
            if (Indices == null)
                Indices = Vertices.Indices();

            // Compute face normals if possible
            if (FaceNormals == null && VertexNormals != null)
                FaceNormals = NumFaces.Select(ComputeFaceNormal);

            if (NumMeshes > 0)
            {
                // Mesh offset is the same as the offset of its first sub-mesh.
                if (MeshSubmeshOffset != null)
                {
                    MeshIndexOffsets = MeshSubmeshOffset.Select(submesh => SubmeshIndexOffsets[submesh]).ToArray();
                    MeshSubmeshCount = GetSubArrayCounts(MeshSubmeshOffset.Count, MeshSubmeshOffset, NumSubmeshes)
                        .ToArray();
                }

                if (MeshIndexOffsets != null)
                {
                    MeshIndexCounts = GetSubArrayCounts(NumMeshes, MeshIndexOffsets, NumCorners);
                }
            }
            else
            {
                MeshSubmeshCount = Array.Empty<int>();
            }

            // Compute for each submesh, how many indices does it use. 
            if (SubmeshIndexOffsets != null)
                SubmeshIndexCount = GetSubArrayCounts(SubmeshIndexOffsets.Count, SubmeshIndexOffsets, NumCorners);

            // Compute structures for the meshes and sub-meshes 
            var meshes = new List<G3dMesh>();
            for (var i = 0; i < NumMeshes; ++i)
            {
                var curSubMesh = MeshSubmeshOffset[i];
                var numSubMeshes = MeshSubmeshCount[i];

                var subMeshes = new G3dSubmesh[numSubMeshes];
                for (var j = 0; j < numSubMeshes; ++j)
                {
                    subMeshes[j] = new G3dSubmesh(this, curSubMesh + j, i);
                }

                meshes.Add(new G3dMesh(this, i, subMeshes));
            }
            Meshes = meshes;

            // Compute all of the materials 
            if (MaterialColors != null)
                Materials = MaterialColors.Count.Select(i => new G3dMaterial(this, i))
                    .ToArray();

            // Update the instance options
            if (InstanceFlags == null)
                InstanceFlags = ((ushort) 0).Repeat(NumInstances).ToArray();
        }

        private static IReadOnlyList<int> GetSubArrayCounts(int numItems, IReadOnlyList<int> offsets, int totalCount)
            => numItems.Select(i => i < (numItems - 1)
                ? offsets[i + 1] - offsets[i]
                : totalCount - offsets[i]).ToArray();

        private static void ValidateSubArrayCounts(IReadOnlyList<int> subArrayCounts, string memberName)
        {
            for (var i = 0; i < subArrayCounts.Count; ++i)
            {
                if (subArrayCounts[i] < 0)
                    throw new Exception($"{memberName}[{i}] is a negative sub array count.");
            }
        }

        public static Vector3 Average(IArray<Vector3> xs)
            => xs.Aggregate(Vector3.Zero, (a, b) => a + b) / xs.Count;

        public Vector3 ComputeFaceNormal(int nFace)
            => Average(NumCornersPerFace.Select(c => VertexNormals[nFace * NumCornersPerFace + c]));

        public static G3D Read(string filePath)
        {
            G3D r = null;
            MemoryMappedView.ReadFile(filePath, view => r = Read(view));
            return r;
        }

        public static G3D ReadFromGeometryBufferIfPossible(BFastReader reader)
        {
            // Check if there is a geometry buffer (e.g., as in a VIM file)
            try
            {
                var tmp = reader.BufferNames.ToList().IndexOf("geometry");
                if (tmp < 0) return null;
                var (name, range) = reader.GetNameAndRange(tmp);
                using (var subView = reader.View.CreateSubView(range.Begin, range.Count))
                {
                    return Read(subView);
                }
            }
            catch 
            {
                return null;
            }
        }

        public static G3D Read(MemoryMappedView view)
        {
            var reader = new BFastReader(view);
            var r = ReadFromGeometryBufferIfPossible(reader);
            if (r != null) 
                return r;

            var header = new G3dHeader();
            var attributes = new List<GeometryAttribute>();

            void OnBuffer(string name, MemoryMappedView subView, int index)
            {
                if (name == "meta")
                {
                    subView.Accessor.Read(0, out header);
                    return;
                }

                if (!AttributeDescriptor.TryParse(name, out var attributeDescriptor))
                    throw new Exception("Failed to parse attribute");

                // Populate a default attribute with the parsed attribute descriptor.
                var defaultAttribute = attributeDescriptor.ToDefaultAttribute(0);
                var geometryAttribute = defaultAttribute.Read(subView);
                attributes.Add(geometryAttribute);
            }

            // Assume it is a normal G3D
            reader.Read(OnBuffer);
            return new G3D(attributes, header);
        }
        
        public static G3D Create(params GeometryAttribute[] attributes)
            => new G3D(attributes);

        public static G3D Create(G3dHeader header, params GeometryAttribute[] attributes)
            => new G3D(attributes, header);
    }
}
