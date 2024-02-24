using System;
using System.Diagnostics;
using Ara3D.Core.Tests;
using Ara3D.Serialization.VIM;
using Ara3D.Serialization.BFAST;
using NUnit.Framework;
using Ara3D.Utils;

namespace Vim.Format.Tests;

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
    }

    [Test]
    public static void BFastLoaderTest()
    {
        var f = Skanska;
        var sw = Stopwatch.StartNew();
        BFastReader.Read(f, (name, view, index)
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
    }

}
