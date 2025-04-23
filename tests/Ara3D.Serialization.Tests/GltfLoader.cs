using System.Numerics;
using glTFLoader;
using SharpGLTF.Geometry;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPosition;

namespace Ara3D.Serialization.Tests;


public class GltfScene
{

}

public class GltfMesh(int id)
{
}

public class GltfNode
{
    public GltfMesh Mesh { get; set; }
}

public static class GltfLoader
{
    public static void LoadUsingKhronos(string filePath)
    {
        var deserializedFile = Interface.LoadModel(filePath);
        Assert.NotNull(deserializedFile);

        // read all buffers
        for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
        {
            var buffer = deserializedFile.Buffers[i];
            Console.WriteLine($"Buffer {i}: {buffer.Uri} is {buffer.ByteLength} bytes");
        }
    }

    public static void LoadUsingSharpGltf(string filePath)
    {
        var model = SharpGLTF.Schema2.ModelRoot.Load(filePath);

        foreach (var scene in model.LogicalScenes)
        {
            Console.WriteLine($"Scene: {scene.Name}");
            foreach (var node in scene.VisualChildren)
            {
                Console.WriteLine($"Node: {node.Name}");
                if (node.Mesh != null)
                {
                    Console.WriteLine($"Mesh: {node.Mesh.Name}");
                }
            }
        }
    }

    public static void WriteUsingSharpGltf(string directory, string name)
    {


        // create two materials

        var material1 = new MaterialBuilder()
            .WithDoubleSide(true)
            .WithMetallicRoughnessShader()
            .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 0, 0, 1));

        var material2 = new MaterialBuilder()
            .WithDoubleSide(true)
            .WithMetallicRoughnessShader()
            .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 0, 1, 1));

        // create a mesh with two primitives, one for each material

        var mesh = new MeshBuilder<VERTEX>("mesh");

        var prim = mesh.UsePrimitive(material1);
        prim.AddTriangle(new VERTEX(-10, 0, 0), new VERTEX(10, 0, 0), new VERTEX(0, 10, 0));
        prim.AddTriangle(new VERTEX(10, 0, 0), new VERTEX(-10, 0, 0), new VERTEX(0, -10, 0));

        prim = mesh.UsePrimitive(material2);
        prim.AddQuadrangle(new VERTEX(-5, 0, 3), new VERTEX(0, -5, 3), new VERTEX(5, 0, 3), new VERTEX(0, 5, 3));

        // create a scene

        var scene = new SharpGLTF.Scenes.SceneBuilder();

        scene.AddRigidMesh(mesh, Matrix4x4.Identity);

        // save the model in different formats

        var model = scene.ToGltf2();

        var filePath = Path.Combine(directory, name);
        model.SaveAsWavefront(filePath + ".obj");
        model.SaveGLB(filePath + "mesh.glb");
        model.SaveGLTF(filePath + "mesh.gltf");
    }
}