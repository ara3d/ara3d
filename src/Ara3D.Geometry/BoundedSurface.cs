using System;
using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    /// <summary>
    /// A bounded surface is a 2D sampling of a surface with
    /// an approximate bounded-box.
    /// The bounding box is intended for use with proportional deformations. 
    /// </summary>
    public interface IBoundedSurface : ISurface
    {
        AABox Bounds { get; }
        ISurface Surface { get; }
    }

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