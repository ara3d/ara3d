using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Ara3D.Utils.Roslyn
{
    /// <summary>
    /// Represents the input and output.
    /// However, we need to compile multiple things. So the service. 
    /// </summary>
    public class Compilation
    {
        public CompilerInput Input { get; }
        public EmitResult EmitResult { get; }
        public CSharpCompilation Compiler { get; }
        public CompilerOptions Options => Input.Options;
        public IReadOnlyList<SemanticModel> SemanticModels { get; }

        public Compilation(CompilerInput input,
            CSharpCompilation compiler,
            EmitResult result)
        {
            Input = input;
            Compiler = compiler;
            EmitResult = result;
            SemanticModels = input.SourceFiles.Select(sf => Compiler?.GetSemanticModel(sf.SyntaxTree)).ToList();
        }
    }

    public static partial class RoslynUtils
    {
        public static Compilation Compile(this CompilerInput input,
            CSharpCompilation compiler = default,
            CancellationToken token = default)
        {
            compiler = compiler ?? CSharpCompilation.Create(
                input.Options.AssemblyName, 
                input.SyntaxTrees,
                input.Options.MetadataReferences, 
                input.Options.CompilationOptions);

            var outputPath = input.Options.OutputFileName;
            outputPath.DeleteAndCreateDirectory();

            using (var peStream = File.OpenWrite(outputPath))
            {
                var emitOptions = new EmitOptions(false, DebugInformationFormat.Embedded);
                var result = compiler.Emit(peStream, null, null, null, null, emitOptions, null, null, input.EmbeddedTexts, token);
                return new Compilation(input, compiler, result);
            }
        }

        public static Compilation Compile(string source, CompilerOptions options = default, CancellationToken token = default)
            => ParseCSharp(source).ToCompilerInput(options).Compile(default, token);

        public static Compilation Compile(this ParsedSourceFile inputFile, CompilerOptions options = default, CancellationToken token = default)
            => inputFile.ToCompilerInput(options).Compile(default, token);

        public static Compilation Compile(this IEnumerable<ParsedSourceFile> inputFiles, CompilerOptions options = default, CancellationToken token = default)
            => inputFiles.ToCompilerInput(options).Compile(default, token);
    }
}
