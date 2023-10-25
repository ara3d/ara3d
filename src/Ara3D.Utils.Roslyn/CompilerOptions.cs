using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerOptions
    {
        public CompilerOptions(IEnumerable<string> fileReferences = null, string outputFileName = null, bool debug = true)
            => (FileReferences, OutputFileName, Debug) = 
                ((fileReferences ?? RoslynUtils.LoadedAssemblyLocations()).ToArray(), outputFileName ?? RoslynUtils.GenerateNewDllFileName(), debug);

        public string OutputFileName { get; }
        public bool Debug { get; }
        public IReadOnlyList<string> FileReferences { get; }

        public string AssemblyName
            => Path.GetFileNameWithoutExtension(OutputFileName);

        public LanguageVersion Language
        {
            get;
            set; 
        } = LanguageVersion.CSharp10;

        public CSharpParseOptions ParseOptions
            => new CSharpParseOptions(Language);

        public CSharpCompilationOptions CompilationOptions
            => new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(true)                
                .WithOptimizationLevel(Debug ? OptimizationLevel.Debug : OptimizationLevel.Release);
    
        public IEnumerable<MetadataReference> MetadataReferences
            => RoslynUtils.ReferencesFromFiles(FileReferences);

        public CompilerOptions WithNewOutputFilePath(string fileName = null)
            => new CompilerOptions(FileReferences, fileName, Debug);
    }
}
