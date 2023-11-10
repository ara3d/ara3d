using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ara3D.Utils;
using Ara3D.Utils.Roslyn;

namespace Ara3D.Bowerbird.Core
{
    /// <summary>
    /// Compiles all
    /// </summary>
    public class FolderCompilation
    {
        public DirectoryPath Directory { get; }
        public List<string> ReferencedAssemblies { get; } = new List<string>();
        public List<FilePath> Files { get; }

        public FolderCompilation(DirectoryPath path)
        {
            Directory = path;
            var refsFile = Directory.RelativeFile("references.txt");
            if (refsFile.Exists())
                ReferencedAssemblies = refsFile.ReadAllLines().ToList();
            Files = Directory.GetFiles("*.cs").ToList();
        }

        public CompilerInput ToCompilerInput(CompilerOptions options, CancellationToken token)
        {
            var parsedFiles = Files.ParseCSharp(options, token);
            return new CompilerInput(parsedFiles, options);
        }

        public Compilation Compile(CompilerOptions options, CancellationToken token)
        {
            var input = ToCompilerInput(options, token);
            var r = input.CompileCSharpStandard(default, token);
            return r;
        }
    }
}