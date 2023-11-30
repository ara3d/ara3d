using System;
using System.Collections.Generic;
using Ara3D.Collections;
using Ara3D.Math;
using Ara3D.Buffers;
using Ara3D.Utils;

namespace Ara3D.Graphics
{
    public enum ElementAssociation
    {
        Vertex,
        Corner,
        HalfEdge,
        Face,
        SubMesh,
        Mesh,
        Instance,
        Material,
        Model,
    }

    public enum PrimitiveType
    {
        Int8,
        Int32,
        Float32,
        Float64,
    }

    public static class Semantics
    {
        public const string Position = "position";
        public const string UV = "uv";
        public const string Normal = "normal";
        public const string Color = "color";
        public const string Index = "index";

        public static List<string> Known = new List<string> { Position, UV, Normal, Color, Index };
    }

    public interface IRenderBuffer : INamedBuffer 
    {
        ElementAssociation Association { get; }
        int Arity { get; }
        string Semantic { get; }
        PrimitiveType PrimitiveType { get; }
    }

    public interface IRenderBuffer<T> : IRenderBuffer
    {
        IArray<T> Array { get; }
    }

    public interface IRenderMesh 
    {
        IArray<IRenderBuffer> Buffers { get; }
        IRenderBuffer<Vector3> PositionBuffer { get; }
        IRenderBuffer<Int3> IndexBuffer { get; }
        IRenderBuffer<Vector3> NormalBuffer { get; }
        IArray<IRenderBuffer<Vector2>> UvBuffers { get; }
        IRenderBuffer<ColorRGBA> ColorBuffer { get; } 
    }

    public interface IRenderInstance
    {
        Matrix4x4 WorldTransform { get; }
        IRenderMesh Mesh { get; }
        IArray<IRenderInstance> Children { get; }
        AABox TransformedBoundingBox { get; }
    }

    public interface IRenderSubMesh
    {
        long StartIndex { get; }
        long IndexCount { get; }
        long BaseVertexOffset { get; }
        IRenderMaterial Material { get; }
        IRenderMesh Mesh { get; }
    }

    public interface IRenderMaterial
    {
        ColorRGBA Color { get; }
        float Roughness { get; }
        float Metallic { get; }
        float Reflectivity { get; }
    }

    public interface IRenderModel
    {
        IArray<IRenderInstance> Instances { get; }
        IArray<IRenderMesh> Meshes { get; }
        IArray<IRenderMaterial> Materials { get; }
        IArray<IRenderSubMesh> Submeshes { get; }
    }

    public class RenderMesh : IRenderMesh
    {
        public IArray<IRenderBuffer> Buffers { get; }
        public IRenderBuffer<Vector3> PositionBuffer { get; }
        public IRenderBuffer<Int3> IndexBuffer { get; }
        public IRenderBuffer<Vector3> NormalBuffer { get; }
        public IArray<IRenderBuffer<Vector2>> UvBuffers { get; }
        public IRenderBuffer<ColorRGBA> ColorBuffer { get; }
        
        public int NumVertices { get; } 
        public int NumFaces { get; } 

        public RenderMesh(IArray<IRenderBuffer> buffers)
        {
            Buffers = buffers.Where(b => b != null).ToIArray();
            var uvs = new ArrayBuilder<IRenderBuffer<Vector2>>();

            foreach (var buffer in Buffers.Enumerate())
            {
                switch (buffer.Semantic)
                {
                    case Semantics.Color:
                    {
                        Verifier.AssertEquals(buffer.PrimitiveType, PrimitiveType.Int8);
                        Verifier.AssertEquals(buffer.Arity, 4);
                        Verifier.AssertEquals(buffer.Association, ElementAssociation.Vertex);
                        Verifier.Assert(ColorBuffer == null, "Color buffer is already assigned");
                        ColorBuffer = buffer.CastTo<ColorRGBA>();
                        break;
                    }
                    case Semantics.Index:
                    {
                        Verifier.AssertEquals(buffer.PrimitiveType, PrimitiveType.Int32);
                        Verifier.AssertEquals(buffer.Arity, 3);
                        Verifier.AssertEquals(buffer.Association, ElementAssociation.Face);
                        Verifier.Assert(IndexBuffer == null, "Index buffer is already assigned");
                        IndexBuffer = buffer.CastTo<Int3>();
                        break;
                    }
                    case Semantics.Normal:
                    {
                        Verifier.AssertEquals(buffer.PrimitiveType, PrimitiveType.Float32);
                        Verifier.AssertEquals(buffer.Arity, 3);
                        Verifier.AssertEquals(buffer.Association, ElementAssociation.Vertex);
                        Verifier.Assert(NormalBuffer == null, "Normal buffer is already assigned");
                        NormalBuffer = buffer.CastTo<Vector3>();
                        break;
                    }
                    case Semantics.UV:
                    {
                        Verifier.AssertEquals(buffer.PrimitiveType, PrimitiveType.Float32);
                        Verifier.AssertEquals(buffer.Arity, 2);
                        Verifier.AssertEquals(buffer.Association, ElementAssociation.Vertex);
                        uvs.Add(buffer.CastTo<Vector2>());
                        break;
                    }
                    case Semantics.Position:
                    {
                        Verifier.AssertEquals(buffer.PrimitiveType, PrimitiveType.Float32);
                        Verifier.AssertEquals(buffer.Arity, 3);
                        Verifier.AssertEquals(buffer.Association, ElementAssociation.Vertex);
                        Verifier.Assert(PositionBuffer == null, "Position buffer is already assigned");
                        PositionBuffer = buffer.CastTo<Vector3>();
                        break;
                    }
                    default:
                        throw new Exception($"Unrecognized semantic {buffer.Semantic}");
                }
            }

            UvBuffers = uvs.ToIArray();
            if (PositionBuffer == null)
                throw new Exception("Required position buffer");
            NumVertices = PositionBuffer.Array.Count;
            if (IndexBuffer == null)
            {
                Verifier.Assert(NumVertices % 3 == 0, $"Number of vertices {NumVertices} must be divisible by three");
                NumFaces = NumVertices / 3;
            }
            else
            {
                NumFaces = IndexBuffer.Array.Count;
            }

            for (var i = 0; i < UvBuffers.Count; ++i)
            {
                var n = UvBuffers[i].GetCount();
                Verifier.AssertEquals(n, NumVertices, $"UvBuffer[{i}].Count");
            }

            if (ColorBuffer != null)
                Verifier.AssertEquals(ColorBuffer.GetCount(), NumVertices, "ColorBuffer.Count");

            if (NormalBuffer != null)
                Verifier.AssertEquals(NormalBuffer.GetCount(), NumVertices, "NormalBuffer.Count");
        }
    }
}
