using Ara3D.Math;
using Ara3D.Serialization.G3D;

namespace Ara3D.Geometry.ToRemove
{
    public interface IMaterial
    {
        Vector4 Color { get; }
        float Smoothness { get; }
        float Glossiness { get; }
    }

    public class G3dMaterialWrapper : IMaterial
    {
        public G3dMaterial Material;
        public G3dMaterialWrapper(G3dMaterial material) => Material = material;
        public Vector4 Color => Material.Color;
        public float Smoothness => Material.Smoothness;
        public float Glossiness => Material.Glossiness;
    }
}
