using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerInput
    {
        public IReadOnlyList<ParsedSourceFile> SourceFiles { get; }
        public CompilerOptions Options { get; }
        public IEnumerable<SyntaxTree> SyntaxTrees => SourceFiles.Select(sf => sf.SyntaxTree);
        public IEnumerable<EmbeddedText> EmbeddedTexts => SourceFiles.Select(sf => sf.EmbeddedText);

        public CompilerInput(IEnumerable<ParsedSourceFile> sourceFiles, CompilerOptions options)
        {
            SourceFiles = sourceFiles.ToList();
            Options = options;
        }
    }

    public static partial class RoslynUtils
    {
        public static CompilerInput ToCompilerInput(this ParsedSourceFile sourceFile,
            CompilerOptions options = default)
            => new[] { sourceFile }.ToCompilerInput(options);

        public static CompilerInput ToCompilerInput(this IEnumerable<ParsedSourceFile> sourceFiles,
            CompilerOptions options = default)
        {
            options = options ?? CompilerOptions.CreateDefault();
            return new CompilerInput(sourceFiles, options);
        }
    }
}