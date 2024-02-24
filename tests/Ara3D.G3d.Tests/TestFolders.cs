using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Utils;
using NUnit.Framework;

namespace Ara3D.Serialization.G3D.Tests;

public static class TestFolders
{
    public static DirectoryPath ThisSourceFolder = SourceCodeLocation.GetFolder();
    public static DirectoryPath RepoRootPath = ThisSourceFolder.GetParent().GetParent();
    public static FilePath SolutionFile = RepoRootPath.RelativeFile("ara3d.sln");
    public static DirectoryPath ShootOutRepo = RepoRootPath.RelativeFolder("..", "3d-format-shootout");
    public static DirectoryPath VimDataFilesDir = ShootOutRepo.RelativeFolder("data", "files", "vim");
    public static IReadOnlyList<FilePath> VimDataFiles => VimDataFilesDir.GetFiles("*.vim").ToList();

    [Test]
    public static void ValidateTestFolders()
    {
        Assert.IsTrue(RepoRootPath.Exists());
        Assert.IsTrue(SolutionFile.Exists());

        // NOTE: this assumes that you have downloaded the "3d-format-shootout" repo 
        // and placed it in 
        Assert.IsTrue(ShootOutRepo.Exists());
        Assert.IsTrue(VimDataFilesDir.Exists());

        foreach (var f in VimDataFiles )
        {
            Console.WriteLine($"Found VIM file {f}");
        }
    }
}