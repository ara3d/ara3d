using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using Ara3D.Serialization.VIM;
using Ara3D.Serialization.BFAST;
using NUnit.Framework;

namespace Vim.Format.Tests;

public static class BigFileTest
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

    public const string File = @"C:\Users\cdigg\Documents\VIM\2023 - Hospital (2).vim";

    [Test]
    public static void G3dTest()
    {
        var sw = Stopwatch.StartNew();
        var vim = Serializer.Deserialize(File);
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");

        var g = vim.Geometry;
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
        //var f = VimFormatRepoPaths.GetLatestWolfordResidenceVim();
        var f = @"C:\Users\cdigg\Documents\VIM\kahua_navis_dedup.vim";

        var sw = Stopwatch.StartNew();
        BFastReader.Read(f, (name, view, index)
            => OutputBFastBuffer(name, view, index, "  "));
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");
    }

    [Test]
    public static void VimLoaderTest()
    {
        //var f = VimFormatRepoPaths.GetLatestWolfordResidenceVim();
        var f = @"C:\Users\cdigg\Documents\VIM\kahua_navis_dedup.vim";
        var sw = Stopwatch.StartNew();
        var vim = Serializer.Deserialize(f);
        
        Console.WriteLine($@"Time to open file {sw.Elapsed.TotalSeconds}");
    }
}
