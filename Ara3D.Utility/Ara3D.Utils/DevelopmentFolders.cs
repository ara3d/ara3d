using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ara3D.Utils
{
    /// <summary>
    /// An instance of the class should be instantiated from a source file that contains the project file.
    /// </summary>
    public class DevelopmentFolders
    {
        public FilePath SourceFile { get; }
        public DirectoryPath SourceFolder => SourceFile.GetDirectory();
        public DirectoryPath ApiKeysFolder => SourceFolder.RelativeFolder("keys");
        public FilePath ProjectFile { get; }

        public DevelopmentFolders([CallerFilePath] string callingFile = null)
        {
            if (callingFile == null)
                throw new Exception("Development folder not provided");
            SourceFile = callingFile;
            ProjectFile = SourceFolder.GetFiles("*.csproj").FirstOrDefault();
            if (ProjectFile == null)
                throw new Exception($"No project file found at {SourceFolder}");
        }
    }
}