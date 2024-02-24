using Ara3D.Utils;
using NUnit.Framework.Legacy;

namespace Ara3D.Core.Tests;

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
        ClassicAssert.IsTrue(RepoRootPath.Exists());
        ClassicAssert.IsTrue(SolutionFile.Exists());

        // NOTE: this assumes that you have downloaded the "3d-format-shootout" repo 
        // and placed it in 
        ClassicAssert.IsTrue(ShootOutRepo.Exists());
        ClassicAssert.IsTrue(VimDataFilesDir.Exists());

        foreach (var f in VimDataFiles )
        {
            Console.WriteLine($"Found VIM file {f}");
        }
    }
}