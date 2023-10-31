using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Ara3D.Utils.Roslyn
{
    public class Compilation
    {
        public IReadOnlyDictionary<string, ScriptFile> InputFileLookup { get; }
        public IEnumerable<ScriptFile> InputsFiles => InputFileLookup.Values;
        public EmitResult EmitResult { get; }
        public CompilerOptions Options { get; }
        public CSharpCompilation Compiler { get; }

        public IReadOnlyList<SemanticModel> SemanticModels { get; }
        public IReadOnlyList<SyntaxTree> SyntaxTrees { get; }

        public Compilation(IEnumerable<ScriptFile> inputFiles = null, CompilerOptions options = default, CSharpCompilation compiler = default, EmitResult result = default)
        {
            InputFileLookup = (inputFiles ?? Array.Empty<ScriptFile>()).ToDictionary(f => f.FilePath, f => f);
            Options = options ?? new CompilerOptions(RoslynUtils.LoadedAssemblyLocations());
            SyntaxTrees = InputFileLookup.Values.Select(f => f.SyntaxTree).ToList();
            Compiler = compiler ?? CSharpCompilation.Create(Options.AssemblyName, SyntaxTrees, Options.MetadataReferences, Options.CompilationOptions);
            SemanticModels = SyntaxTrees.Select(st => Compiler?.GetSemanticModel(st)).ToList();
            EmitResult = result;
        }

        public static Compilation Create(IEnumerable<ScriptFile> inputFiles = null,
            CompilerOptions options = default, CSharpCompilation compiler = default, EmitResult result = default)
            => new Compilation(inputFiles, options, compiler, result);

        public static Compilation Create(IEnumerable<string> inputFiles, CompilerOptions options = default, CSharpCompilation compiler = default, EmitResult result = default)
            => new Compilation(inputFiles.Select(f => ScriptFile.Create(f, options?.ParseOptions)), options, compiler, result);
    }
}
