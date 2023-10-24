using System;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Math;

namespace Ara3D.Graphics
{
    public unsafe interface IBuffer
    {
        byte* Data { get; }
        long Length { get; }
    }

    public interface ITypedBuffer
    {
        Type Type { get; }
        long NumElements { get; }
        long ElementSize { get; }
    }

    public interface INamed
    {
        string Name { get; }
    }

    public interface INamedBuffer : INamed, IBuffer { }
    
    public interface INamedTypedBuffer : INamed, IBuffer { }

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

    public interface IRenderBuffer : INamedTypedBuffer 
    {
        ElementAssociation Association { get; }
        int Arity { get; }
        int Index { get; }
        string Semantic { get; }
    }

    public interface IRenderBuffer<T> : IRenderBuffer
    {
        IArray<T> Array { get; }
    }

    public interface IRenderMesh 
    {
        IRenderBuffer<Vector3> VertexBuffer { get; }
        IRenderBuffer<int> IndexBuffer { get; }
        IRenderBuffer<Vector3> NormalBuffer { get; }
        IArray<IRenderBuffer<Vector2>> UvBuffers { get; }
    }
    
    public interface IRenderData
    {
        IArray<IRenderBuffer> Attributes { get; }
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

    public static class Extensions
    {
        public static IMesh ToIMesh(this IRenderMesh renderMesh)
            => renderMesh.VertexBuffer.Array.TriMesh(renderMesh.IndexBuffer.Array);
    }
}
