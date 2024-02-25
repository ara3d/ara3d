using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Serialization.BFAST;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Serialization.G3D
{
    /// <summary>
    /// Represents a basic single-precision G3D in memory, with access to common attributes.
    /// The G3D format can be double precision, but this data structure won't provide access to all of the attributes.
    /// In the case of G3D formats that are non-conformant to the expected semantics you can use GeometryAttributes.
    /// This class is inspired heavily by the structure of FBX and Assimp. 
    /// </summary>
    public class G3D : GeometryAttributes
    {
        public new static G3D Empty = Create();

        public G3dHeader Header { get; }

        // These are the values of the most common attributes. Some are retrieved directly from data, others are computed on demand, or coerced. 

        // Vertex buffer. Usually present.
        public IArray<Vector3> Vertices { get; }

        // Index buffer (one index per corner, and per half-edge). Computed if absent. 
        public IArray<int> Indices { get; }

        // Vertex associated data, provided or null
        public List<IArray<Vector2>> AllVertexUvs { get; } = new List<IArray<Vector2>>();
        public List<IArray<Vector4>> AllVertexColors { get; } = new List<IArray<Vector4>>();
        public IArray<Vector2> VertexUvs => AllVertexUvs?.ElementAtOrDefault(0);
        public IArray<Vector4> VertexColors => AllVertexColors?.ElementAtOrDefault(0);
        public IArray<Vector3> VertexNormals { get; }
        public IArray<Vector4> VertexTangents { get; }

        // Faces
        public IArray<int> FaceMaterials { get; } // Material indices per face, 
        public IArray<Vector3> FaceNormals { get; } // If not provided, are computed dynamically as the average of all vertex normals,

        // Meshes
        public IArray<int> MeshIndexOffsets { get; } // Offset into the index buffer for each Mesh
        public IArray<int> MeshVertexOffsets { get; } // Offset into the vertex buffer for each Mesh
        public IArray<int> MeshIndexCounts { get; } // Computed
        public IArray<int> MeshVertexCounts { get; } // Computed
        public IArray<int> MeshSubmeshOffset { get; }
        public IArray<int> MeshSubmeshCount { get; } // Computed
        public IArray<G3dMesh> Meshes { get; }

        // Instances
        public IArray<int> InstanceParents { get; } // Index of the parent transform 
        public IArray<Matrix4x4> InstanceTransforms { get; } // A 4x4 matrix in row-column order defining the transormed
        public IArray<int> InstanceMeshes { get; } // The SubGeometry associated with the index
        public IArray<ushort> InstanceFlags { get; } // The instance flags associated with the index.

        // Shapes
        public IArray<Vector3> ShapeVertices { get; }
        public IArray<int> ShapeVertexOffsets { get; }
        public IArray<Vector4> ShapeColors { get; }
        public IArray<float> ShapeWidths { get; }
        public IArray<int> ShapeVertexCounts { get; } // Computed
        public IArray<G3dShape> Shapes { get; } // Computed

        // Materials
        public IArray<Vector4> MaterialColors { get; } // RGBA with transparency.
        public IArray<float> MaterialGlossiness { get; }
        public IArray<float> MaterialSmoothness { get; }
        public IArray<G3dMaterial> Materials { get; }

        // Submeshes
        public IArray<int> SubmeshIndexOffsets { get; }
        public IArray<int> SubmeshIndexCount { get; }
        public IArray<int> SubmeshMaterials { get; }

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
                        if (attr.IsTypeAndAssociation<Vector3>(Association.assoc_shapevertex))
                            ShapeVertices = ShapeVertices ?? attr.AsType<Vector3>().Data;
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
                        if (desc.Association == Association.assoc_shape)
                            ShapeColors = ShapeColors ?? attr.AttributeToColors();
                        if (desc.Association == Association.assoc_material)
                            MaterialColors = MaterialColors ?? attr.AttributeToColors();
                        break;

                    case Semantic.VertexOffset:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_mesh))
                            MeshVertexOffsets = MeshVertexOffsets ?? attr.AsType<int>().Data;
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_shape))
                            ShapeVertexOffsets = ShapeVertexOffsets ?? attr.AsType<int>().Data;
                        break;

                    case Semantic.IndexOffset:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_mesh))
                            MeshIndexOffsets = MeshIndexOffsets ?? attr.AsType<int>().Data;
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_submesh))
                            SubmeshIndexOffsets = SubmeshIndexOffsets ?? attr.AsType<int>().Data;
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
                            SubmeshMaterials = SubmeshMaterials ?? attr.AsType<int>().Data;
                        break;

                    case Semantic.Parent:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_instance))
                            InstanceParents = InstanceParents ?? attr.AsType<int>().Data;
                        break;

                    case Semantic.Mesh:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_instance))
                            InstanceMeshes = InstanceMeshes ?? attr.AsType<int>().Data;
                        break;

                    case Semantic.Transform:
                        if (attr.IsTypeAndAssociation<Matrix4x4>(Association.assoc_instance))
                            InstanceTransforms = InstanceTransforms ?? attr.AsType<Matrix4x4>().Data;
                        break;

                    case Semantic.Width:
                        if (attr.IsTypeAndAssociation<float>(Association.assoc_shape))
                            ShapeWidths = ShapeWidths ?? attr.AsType<float>().Data;
                        break;

                    case Semantic.Glossiness:
                        if (attr.IsTypeAndAssociation<float>(Association.assoc_material))
                            MaterialGlossiness = attr.AsType<float>().Data;
                        break;

                    case Semantic.Smoothness:
                        if (attr.IsTypeAndAssociation<float>(Association.assoc_material))
                            MaterialSmoothness = attr.AsType<float>().Data;
                        break;

                    case Semantic.SubMeshOffset:
                        if (attr.IsTypeAndAssociation<int>(Association.assoc_mesh))
                            MeshSubmeshOffset = attr.AsType<int>().Data;
                        break;

                    case Semantic.Flags:
                        if (attr.IsTypeAndAssociation<ushort>(Association.assoc_instance))
                            InstanceFlags = attr.AsType<ushort>().Data;
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
                // Mesh offset is the same as the offset of its first submesh.
                if(MeshSubmeshOffset != null)
                {
                    MeshIndexOffsets = MeshSubmeshOffset.Select(submesh => SubmeshIndexOffsets[submesh]);
                    MeshSubmeshCount = GetSubArrayCounts(MeshSubmeshOffset.Count, MeshSubmeshOffset, NumSubmeshes).Evaluate();
                }

                if(MeshIndexOffsets != null)
                {
                    MeshIndexCounts = GetSubArrayCounts(NumMeshes, MeshIndexOffsets, NumCorners);
                    MeshVertexOffsets = MeshIndexOffsets
                        .Zip(MeshIndexCounts, (start, count) => (start, count))
                        .Select(range => Indices.SubArray(range.start, range.count).Min());
                }
        
                if (MeshVertexOffsets != null)
                    MeshVertexCounts = GetSubArrayCounts(NumMeshes, MeshVertexOffsets, NumVertices);
            }
            else
            {
                MeshSubmeshCount = Array.Empty<int>().ToIArray();
            }

            if (SubmeshIndexOffsets != null)
                SubmeshIndexCount = GetSubArrayCounts(SubmeshIndexOffsets.Count, SubmeshIndexOffsets, NumCorners).Evaluate();

            // Compute all meshes
            Meshes = NumMeshes.Select(i => new G3dMesh(this, i));

            if (MaterialColors != null)
                Materials = MaterialColors.Count.Select(i => new G3dMaterial(this, i));

            // Process the shape data
            if (ShapeVertices == null)
                ShapeVertices = Vector3.Zero.Repeat(0);

            if (ShapeVertexOffsets == null)
                ShapeVertexOffsets = Array.Empty<int>().ToIArray();

            if (ShapeColors == null)
                ShapeColors = Vector4.Zero.Repeat(0);
             
            if (ShapeWidths == null)
                ShapeWidths = Array.Empty<float>().ToIArray();

            // Update the instance options
            if (InstanceFlags == null)
                InstanceFlags = ((ushort) 0).Repeat(NumInstances);

            ShapeVertexCounts = GetSubArrayCounts(NumShapes, ShapeVertexOffsets, ShapeVertices.Count);
            ValidateSubArrayCounts(ShapeVertexCounts, nameof(ShapeVertexCounts));

            Shapes = NumShapes.Select(i => new G3dShape(this, i));
        }

        private static IArray<int> GetSubArrayCounts(int numItems, IArray<int> offsets, int totalCount)
            => numItems.Select(i => i < (numItems - 1)
                ? offsets[i + 1] - offsets[i]
                : totalCount - offsets[i]);

        private static void ValidateSubArrayCounts(IArray<int> subArrayCounts, string memberName)
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
