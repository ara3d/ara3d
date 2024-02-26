using System;
using System.Diagnostics;
using Ara3D.Collections;
using Ara3D.Serialization.BFAST;
using Ara3D.Serialization.VIM;
using Ara3D.UnityBridge;
using Ara3D.Utils;
using NUnit.Framework;

namespace Ara3D.Serialization.G3D.Tests;

public static class VimTests
{
    public static void OutputBFastBuffer(string name, MemoryMappedView view, int index, string indent = "")
    {
        Console.WriteLine($@"{indent} {index}:{name} [{view.Offset}, {view.Offset + view.Size}]");
        if (view.IsBFast())
        {
            BFastReader.Read(view, (name1, view1, index1) 
                => OutputBFastBuffer(name1, view1, index1, indent + "  "));
        }
    }

    public static FilePath Skanska 
        => TestFolders.VimDataFilesDir.RelativeFile("skanska.vim");

    public static void OutputAttributes(G3D g3d)
    {
        var attrs = g3d.Attributes;
        foreach (var att in attrs.ToEnumerable())
        {
            Console.WriteLine($"Attribute: {att.Descriptor}, # elements = {att.ElementCount}");
        }
    }

    public static void OutputVimGeometryData(SerializableDocument doc)
    {
        var g = doc.Geometry;
        Console.WriteLine($"number of meshes {g.Meshes.Count}");
        Console.WriteLine($"number of vertices {g.Vertices.Count}");
        Console.WriteLine($"number of uvs {g.AllVertexUvs.Count}");
        Console.WriteLine($"number of sub-meshes {g.SubmeshIndexCount.Count}");
        Console.WriteLine($"number of faces {g.Indices.Count / 3}");
        Console.WriteLine($"number of instances {g.InstanceMeshes.Count}");
        Console.WriteLine($"number of materials {g.Materials.Count}");
        Console.WriteLine($"number of submesh materials {g.SubmeshMaterials.Count} ");
        OutputAttributes(g);
    }

    [Test]
    public static void BFastLoaderTest()
    {
        var f = Skanska;
        var sw = Stopwatch.StartNew();
        BFastReader.Read((string)f, (name, view, index)
            => OutputBFastBuffer(name, view, index, "  "));
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");
    }

    [Test]
    public static void VimTest()
    {
        var sw = Stopwatch.StartNew();
        var vim = Serializer.Deserialize(Skanska);
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");
        OutputVimGeometryData(vim);
        ValidateG3d(vim.Geometry);
    }

    public static void ValidateG3d(G3D g)
    {
        Assert.IsTrue(g.Indices.All(i => i >= 0 && i < g.NumVertices));
        foreach (var m in g.Meshes)
        {
            ValidateG3dMesh(g, m);
        }
    }

    public static void ValidateG3dMesh(G3D g, G3dMesh m)
    {
        Assert.IsTrue(m.NumCorners > 0);
        Assert.IsTrue(m.VertexOffset >= 0);
        Assert.IsTrue(m.VertexOffset < g.Vertices.Count);
        Assert.IsTrue(m.IndexOffset >= 0);
        Assert.IsTrue(m.IndexOffset < g.NumCorners);
        Assert.IsTrue(m.Submeshes.Count > 0);
        Assert.IsTrue(m.Indices.All(i => i >= 0));
        Assert.IsTrue(m.Indices.All(i => i < m.NumVertices));
    }

    [Test]
    public static void UnityInteropTest()
    {
        var sw = Stopwatch.StartNew();
        var vim = Serializer.Deserialize(Skanska);
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");
        OutputVimGeometryData(vim);
        sw.Restart();
    }
    
}
