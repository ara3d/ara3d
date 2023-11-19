using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ara3D.Utils;
using Ara3D.Utils.Roslyn;

namespace Ara3D.Bowerbird.Core
{
    /// <summary>
    /// Used to compile a directory. Recreate this if there is a change to a folder. 
    /// </summary>
    public class FolderCompilation
    {
        public DirectoryPath Directory { get; }
        public List<string> ExplicitReferences { get; private set; } = new List<string>();
        public List<FilePath> Files { get; private set; }
        
        public CompilerOptions CompilerOptions { get; private set; }
        public CompilerInput CompilerInput { get; private set; }
        public Compilation Compilation { get; private set; }

        public bool Success => Compilation.EmitResult.Success;

        public FolderCompilation(DirectoryPath path)
        {
            Directory = path;
        }

        public void ScanDirectory()
        {
            if (!Directory.Exists())
                throw new Exception($"Could not find directory {Directory}");
            var refsFile = Directory.RelativeFile("references.txt");
            ExplicitReferences = refsFile.Exists() ? refsFile.ReadAllLines().ToList() : new List<string>();
            Files = Directory.GetFiles("*.cs").ToList();
        }

        public CompilerOptions ComputeCompilerOptions()
        {
            // TODO: a special cache folder should be generated for generated roslyn DLLs. 
            var outputFile = RoslynUtils.GenerateNewDllFileName();

            // TODO: a more sophisticated assembly look-up procedure would be appreciated.
            var refs = ExplicitReferences.Select(f => new FilePath(f));

            refs = refs.Concat(RoslynUtils.LoadedAssemblyLocations());
            return CompilerOptions = new CompilerOptions(refs, outputFile, true);
        }

        public void ParseFiles(CancellationToken token)
        {
            if (CompilerOptions == null)
                throw new Exception("Compiler options are null");
            var parsedFiles = Files.ParseCSharp(CompilerOptions, token);
            CompilerInput = new CompilerInput(parsedFiles, CompilerOptions);
        }

        public Compilation Compile(ILogger logger = default, CancellationToken token = default)
        {
            logger?.Log($"Scanning directory: {Directory}");
            ScanDirectory();
            logger?.Log($"Found {Files.Count} input source files");
            ComputeCompilerOptions();
            logger?.Log("Parsing source files");
            ParseFiles(token);
            logger?.Log("Starting compilation");
            return Compilation = CompilerInput.CompileCSharpStandard(default, token);
        }
    }
}