using System;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IDeformable<out TSelf>
    {
         TSelf Deform(Func<Vector3, Vector3> f);
    }

    public interface IMesh 
        : IGeometry, ITransformable<IMesh>, IDeformable<IMesh>
    {
        IArray<Int3> Faces { get; }
        IArray<Vector3> Vertices { get; }

        IArray<Vector3> Normals { get;}
        IArray<Vector2> UVs { get; }
    }

    public abstract class MeshImpl<TFace, TVertex, TMesh> :  
        IMesh where TMesh : class, IGeometry, IMesh
    {
        protected MeshImpl(IArray<TVertex> vertexData, IArray<TFace> faceData)
            => (VertexData, FaceData) = (vertexData, faceData);

        public IArray<TVertex> VertexData { get; }
        public IArray<TFace> FaceData { get; }

        public abstract IArray<Int3> Faces { get; }
        public abstract IArray<Vector3> Vertices { get; }
        public abstract IArray<Vector2> UVs { get; }
        public abstract IArray<Vector3> Normals { get; }

        public virtual TMesh Transform(Matrix4x4 mat)
            => Deform(v => v.Transform(mat));

        public abstract TMesh Deform(Func<Vector3, Vector3> f);

        IGeometry ITransformable<IGeometry>.Transform(Matrix4x4 mat)
            => Transform(mat);

        IGeometry IDeformable<IGeometry>.Deform(Func<Vector3, Vector3> f)
            => Deform(f);

        IMesh ITransformable<IMesh>.Transform(Matrix4x4 mat)
            => Transform(mat);

        IMesh IDeformable<IMesh>.Deform(Func<Vector3, Vector3> f)
            => Deform(f);
    }

    public class TriMesh : MeshImpl<Int3, Vector3, TriMesh>, 
        IMesh, ITransformable<TriMesh>, IDeformable<TriMesh>
    {
        public TriMesh(IArray<Vector3> vertices, IArray<Int3> faces)
            : base(vertices, faces) { }

        public override IArray<Int3> Faces => FaceData;
        public override IArray<Vector3> Vertices => VertexData;
        public override IArray<Vector2> UVs => null;
        public override IArray<Vector3> Normals => null;

        public override TriMesh Deform(Func<Vector3, Vector3> deform)
            => new TriMesh(Vertices.Select(deform), FaceData);
    }

    public class QuadMesh 
        : MeshImpl<Int4, Vector3, QuadMesh>, IMesh, ITransformable<QuadMesh>, IDeformable<QuadMesh>
    {
        public QuadMesh(IArray<Vector3> vertices, IArray<Int4> faces)
            : base(vertices, faces) { }

        public override IArray<Int3> Faces => FaceData.SelectMany(f => 
           new Tuple<Int3, Int3>((f.X, f.Y, f.Z), (f.Z, f.W, f.X)));

        public override IArray<Vector3> Vertices => VertexData;
        public override IArray<Vector2> UVs => null;
        public override IArray<Vector3> Normals => null;

        public override QuadMesh Deform(Func<Vector3, Vector3> deform)
            => new QuadMesh(Vertices.Select(deform), FaceData);
    }

    public class Surface : MeshImpl<Int4, SurfacePoint, Surface>, IMesh, ITransformable<Surface>, IDeformable<Surface>
    {
        public Surface(IArray<SurfacePoint> vertices, IArray<Int4> faces)
            : base(vertices, faces) { }

        public override IArray<Int3> Faces => FaceData.SelectMany(f =>
            new Tuple<Int3, Int3>((f.X, f.Y, f.Z), (f.Z, f.W, f.X)));

        public override IArray<Vector3> Vertices => VertexData.Select(v => v.Position);
        public override IArray<Vector2> UVs => VertexData.Select(v => v.UV);
        public override IArray<Vector3> Normals => VertexData.Select(v => v.Normal);

        public override Surface Transform(Matrix4x4 mat)
            => new Surface(VertexData.Select(sp =>
                    new SurfacePoint(sp.UV, sp.Position.Transform(mat), sp.Normal.TransformNormal(mat))),
                FaceData);

        public override Surface Deform(Func<Vector3, Vector3> f)
            => new Surface(VertexData.Select(sp =>
                    new SurfacePoint(sp.UV, f(sp.Position), sp.Normal)),
                FaceData);
    }
}
