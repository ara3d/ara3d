using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;

namespace Ara3D.Graphics
{
    public static class RenderMeshExtensions
    {
        // TODO: eventually I want to support reinterpret casts.
        public static IRenderBuffer<T> CastTo<T>(this IRenderBuffer buffer)
            => (IRenderBuffer<T>)buffer;

        public static IRenderBuffer<T> ToRenderBuffer<T>(this IBuffer buffer, string semantic)
            where T: unmanaged
            => new RenderBuffer<T>(buffer, semantic);

        public static IBuffer ToBuffer<T>(this IArray<T> self)
            where T: unmanaged
            => new Buffer<T>(self.ToArray());

        public static IRenderBuffer<T> ToRenderBuffer<T>(this IArray<T> array, string semantic)
            where T : unmanaged
            => array.ToBuffer().ToRenderBuffer<T>(semantic);

        public static IRenderBuffer<Vector3> ToVertexBuffer(this IBuffer points)
            => points.ToRenderBuffer<Vector3>(Semantics.Position);

        public static IRenderBuffer<Vector3> ToNormalBuffer(this IBuffer points)
            => points.ToRenderBuffer<Vector3>(Semantics.Normal);

        public static IRenderBuffer<Vector2> ToUvBuffer(this IBuffer points)
            => points.ToRenderBuffer<Vector2>(Semantics.UV);

        public static IRenderBuffer<Int3> ToIndexBuffer(this IBuffer indices)
            => indices.ToRenderBuffer<Int3>(Semantics.Index);

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
                    triMesh.FaceIndices?.ToIndexBuffer()));

    }
}