using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Ara3D.Collections;
using Ara3D.Interop.WPF;
using Ara3D.Serialization.G3D;
using Ara3D.Utils;
using HelixToolkit.Wpf;

namespace Wpf3DViewer;

public static class ModelLoader
{
    public static Model3DGroup Load(FilePath filePath)
    {
        var mi = new ModelImporter();
        if (filePath.HasExtension(".g3d") || filePath.HasExtension(".vim"))
            return LoadG3D(filePath);
        return mi.Load(filePath);
    }

    public static Model3DGroup LoadG3D(FilePath filePath)
    {
        return G3D.Read(filePath).ToModelGroup();
    }
    
    public static Model3DGroup ToModelGroup(this G3D g3D)
    {
        var r = new Model3DGroup();
        var meshes = g3D.Meshes.Select(m => m.ToMeshGeometry3D()).Evaluate();
        var material = new DiffuseMaterial(Brushes.Silver);
        var models = g3D.InstanceMeshes
            .Where(i => i >= 0)
            .Select(i => meshes[i].ToWpfModel3D(material, g3D.InstanceTransforms[i]))
            .ToList();

        r.Children = new Model3DCollection(models);
        return r;
    }
    
}