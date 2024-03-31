using Ara3D.Serialization.FBX;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;
using Ara3D.Serialization.G3D;

namespace FBXtoG3D
{
    public static class Program
    {
        public static ITriMesh ToIMesh(this DotNetFbxMesh self)
            => Meshes.ToTriMesh(self.Vertices.ToIArray(), self.Indices.ToIArray());

        public static IArray<ITriMesh> GetMeshes(this DotNetFbxScene self)
            => self.Meshes.ToIArray().Select(m => m.ToIMesh());

        public static IArray<(int, Matrix4x4)> GetNodes(this DotNetFbxScene self)
            => self.Nodes.ToIArray().Select(n => (n.MeshIndex, n.Matrix));

        public static void ConvertFbxFileToG3DFile(string inputFile, string outputFile)
        {
            var scene = DotNetFbxScene.Load(inputFile);
            var builder = FbxSceneToG3DBuilder(scene);
            builder.ToG3D().Write(outputFile);
        }

        public static G3DBuilder FbxSceneToG3DBuilder(DotNetFbxScene scene)
        {
            Console.WriteLine($"Application name {scene.ApplicationName} version {scene.ApplicationVersion} vendor {scene.ApplicationVendor}");
            Console.WriteLine($"File FBX Version {scene.FileVersionMajor}.{scene.FileVersionMinor}.{scene.FileVersionRevision}");
            Console.WriteLine($"Author {scene.Author}");
            Console.WriteLine($"Comment {scene.Comment}");
            Console.WriteLine($"Keywords {scene.Keywords}");
            Console.WriteLine($"# Materials {scene.Materials.Count}");
            Console.WriteLine($"# Meshes {scene.Meshes.Count}");
            Console.WriteLine($"# Nodes {scene.Nodes.Count}");
            Console.WriteLine($"Revision {scene.Revision}");
            Console.WriteLine($"Subject {scene.Subject}");
            Console.WriteLine($"Title {scene.Title}");
            
            var r = new G3DBuilder();
            
            foreach (var m in scene.Materials.Values)
            {
                Console.WriteLine($"Material: {m.Index} named {m.Name} has {m.Diffuse} diffuse color");
            }

            foreach (var n in scene.Nodes)
            {
                Console.WriteLine($"Node: {n.Index} named {n.Name} has {n.MeshIndex} mesh");
            }

            foreach (var m in scene.Meshes)
            {
                Console.WriteLine($"Mesh has {m.Indices.Count} indices, and {m.Vertices.Count} vertices");
            }

            return r;
        }

        static void Main(string[] args)
        {
            var inputFile = args[0];
            var outputFile = Path.ChangeExtension(inputFile, "g3d");
            ConvertFbxFileToG3DFile(inputFile, outputFile);
        }
    }
}

