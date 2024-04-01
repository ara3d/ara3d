using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    public class BoundedSurface : IBoundedSurface
    {
        public bool ClosedX => false;
        public bool ClosedY => false;
        public AABox Bounds { get; }
        public ISurface Surface { get; }

        public BoundedSurface(ISurface surface, AABox bounds)
            => (Bounds, Surface) = (bounds, surface);

        public ITransformable TransformImpl(Matrix4x4 mat)
            => DeformImpl(v => v.Transform(mat));

        public IDeformable DeformImpl(Func<Vector3, Vector3> f)
            => new BoundedSurface(Surface.Deform(f), Bounds);
    }
}