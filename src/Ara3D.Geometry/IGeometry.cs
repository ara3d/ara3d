using Ara3D.Math;

namespace Ara3D.Geometry
{
    public interface IGeometry : 
        ITransformable<IGeometry>, 
        IDeformable<IGeometry>
    {
    }
}