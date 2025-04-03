using System;
using System.Collections.Generic;
using System.Numerics;
using Ara3D.Collections;

namespace Ara3D.Serialization.G3D
{
    /// <summary>
    /// A G3dMesh is a section of the G3D data that defines a mesh.
    /// </summary>
    public class G3dMesh
    {
        public readonly int Index;
        public readonly int IndexOffset;
        public readonly int NumCorners;
        public readonly int FaceOffset;
        public readonly int NumFaces;
        public readonly int NumVertices;
        public readonly int VertexOffset;
        public IReadOnlyList<G3dSubmesh> Submeshes;

        // Vertex buffer slice containing only the used vertices .
        public IArray<Vector3> Vertices { get; }

        // Index buffer (one index per corner, and per half-edge)
        // Computed based on the sliced vertices  
        public IArray<int> Indices { get; }

        // Vertex associated data, also sliced 
        public IArray<Vector2> VertexUvs { get; }
        public IArray<Vector3> VertexNormals { get; }
        public IArray<Vector4> VertexColors { get; }
        public IArray<Vector4> VertexTangents { get; }

        // Face asssociated data.
        public IArray<Vector3> FaceNormals { get; }

        public G3dMesh(G3D g3d, int index, IReadOnlyList<G3dSubmesh> subMeshes)
        {
            if (g3d.NumCornersPerFace != 3)
                throw new Exception("Only triangular meshes supported");

            Index = index;
            Submeshes = subMeshes;

            IndexOffset = g3d.MeshIndexOffsets[Index];
            NumCorners = g3d.MeshIndexCounts[Index];
            FaceOffset = IndexOffset / 3;
            NumFaces = NumCorners / 3;

            // NOTE: accessing this data is slow. 
            // There is a lot of indirection, and a lot of function calls.

            var lowestIndex = int.MaxValue;
            var highestIndex = -1;
            for (var i = 0; i < NumCorners; ++i)
            {
                var curIndex = g3d.Indices[i + IndexOffset];
                if (curIndex < lowestIndex)
                    lowestIndex = curIndex;
                if (curIndex > highestIndex)
                    highestIndex = curIndex;
            }

            // These are the offset indices into a subset of the vertex array 
            VertexOffset = lowestIndex;
            NumVertices = highestIndex - lowestIndex + 1;
            var offset = VertexOffset;
            Indices = g3d.Indices.ToIArray().SubArray(IndexOffset, NumCorners).Select(i => i - offset);

            // Compute a subset of the vertices
            Vertices = g3d.Vertices?.ToIArray().SubArray(VertexOffset, NumVertices);
            VertexUvs = g3d.VertexUvs?.ToIArray().SubArray(VertexOffset, NumVertices);
            VertexNormals = g3d.VertexNormals?.ToIArray().SubArray(VertexOffset, NumVertices);
            VertexColors = g3d.VertexColors?.ToIArray().SubArray(VertexOffset, NumVertices);
            VertexTangents = g3d.VertexTangents?.ToIArray().SubArray(VertexOffset, NumVertices);
            FaceNormals = g3d.FaceNormals?.ToIArray().SubArray(FaceOffset, NumFaces);
        }
    }
}
