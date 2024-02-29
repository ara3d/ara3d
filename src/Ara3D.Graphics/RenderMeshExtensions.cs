using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Math;

namespace Ara3D.Graphics
{
    public static class RenderMeshExtensions
    {
        // TODO: eventually I want to support reinterpret casts.
        public static IRenderBuffer<T> CastTo<T>(this IRenderBuffer buffer)
            => (IRenderBuffer<T>)buffer;

        public static int GetCount<T>(this IRenderBuffer<T> buffer)
            => buffer.Array.Count;

        public static IRenderBuffer<T> ToRenderBuffer<T>(this IBuffer<T> buffer, string semantic)
            where T: unmanaged
            => new RenderBuffer<T>(buffer, semantic);

        public static IBuffer<T> ToBuffer<T>(this IArray<T> self)
            where T: unmanaged
            => new Buffer<T>(self.ToArray());

        public static IRenderBuffer<T> ToRenderBuffer<T>(this IArray<T> array, string semantic)
            where T : unmanaged
            => array.ToBuffer().ToRenderBuffer(semantic);

        public static IRenderBuffer<Vector3> ToVertexBuffer(this IBuffer<Vector3> points)
            => points.ToRenderBuffer(Semantics.Position);

        public static IRenderBuffer<Vector3> ToNormalBuffer(this IBuffer<Vector3> points)
            => points.ToRenderBuffer(Semantics.Normal);

        public static IRenderBuffer<Vector2> ToUvBuffer(this IBuffer<Vector2> points)
            => points.ToRenderBuffer(Semantics.UV);

        public static IRenderBuffer<Int3> ToIndexBuffer(this IBuffer<Int3> indices)
            => indices.ToRenderBuffer(Semantics.Index);

        public static IRenderBuffer<Vector3> ToVertexBuffer(this IArray<Vector3> points)
            => points.ToBuffer().ToVertexBuffer();

        public static IRenderBuffer<Vector3> ToNormalBuffer(this IArray<Vector3> points)
            => points.ToRenderBuffer(Semantics.Normal);

        public static IRenderBuffer<Vector2> ToUvBuffer(this IArray<Vector2> points)
            => points.ToRenderBuffer(Semantics.UV);

        public static IRenderBuffer ToIndexBuffer(this IArray<Int3> indices)
            => indices.ToBuffer().ToIndexBuffer();

        public static IRenderMesh ToRenderMesh(this ITriMesh triMesh)
            => new RenderMesh(
                LinqArray.Create(
                    triMesh.Points?.ToVertexBuffer(), 
                    triMesh.Indices?.ToIndexBuffer()));

        public static ITriMesh ToIMesh(this IRenderMesh renderMesh)
            => renderMesh.PositionBuffer.Array.ToTriMesh(renderMesh.IndexBuffer.Array);

        public static IRenderMesh AddBuffer(this IRenderMesh renderMesh, IRenderBuffer buffer)
            => new RenderMesh(renderMesh.Buffers.Append(buffer));

        public static int GetNumFaces(this IRenderMesh self)
            => self.IndexBuffer.GetCount();

        public static int GetNumVertices(this IRenderMesh self)
            => self.PositionBuffer.GetCount();
    }
}