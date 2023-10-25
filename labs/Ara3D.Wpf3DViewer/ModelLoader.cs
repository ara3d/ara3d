using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Ara3D.Serialization.G3D;
using Ara3D.Utils;
using HelixToolkit.Wpf;

namespace ModelViewer;

public static class ModelLoader
{
    public static Model3DGroup Load(FilePath filePath, Dispatcher dispatcher, bool freeze)
    {
        var mi = new ModelImporter();
        if (filePath.HasExtension(".g3d") || filePath.HasExtension(".vim"))
            return LoadG3D(filePath);
        return mi.Load(filePath, dispatcher, freeze);
    }

    public static Model3DGroup LoadG3D(FilePath filePath)
    {
        return G3D.Read(filePath).ToModelGroup();
    }

    public static Model3DGroup ToModelGroup(this G3D g3D)
    {
        g3D.Meshes;
        g3D.InstanceTransforms;
    }

    public static Mesh3D ToMesh(this G3dMesh mesh)
    {

    }
}