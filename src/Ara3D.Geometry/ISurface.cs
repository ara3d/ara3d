namespace Ara3D.Geometry
{
    /// <summary>
    /// A surface is a 3-dimensional shape with no volume.
    /// Some examples of surfaces are: plane, sphere, cylinder, cone, torus.  
    /// </summary>
    public interface ISurface : IGeometry
    {
        bool ClosedX { get; }
        bool ClosedY { get; }
    }
}