using Ara3D.Mathematics;

namespace Ara3D.Geometry
{
    /// <summary>
    /// A parametric surface maps UV coordinates to 3-dimensional points in space.
    /// Any explicit surface can be combined with a parametric surface by interpreting
    /// the normals.  
    /// </summary>
    public interface IParametricSurface : IProcedural<Vector2, Vector3>, ISurface
    {
    }

    public enum StandardPlane
    {
        Xy,
        Xz,
        Yz,
    }
}