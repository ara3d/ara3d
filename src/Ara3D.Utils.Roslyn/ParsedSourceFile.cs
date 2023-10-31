using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Ara3D.Utils.Roslyn
{
    /// <summary>
    /// Contains C# source code, the associated syntax tree, and the source file path.
    /// If not source path is provided then it creates a temp file. 
    /// It also creates an embedded text for the purpose of creating PDBs. 
    /// </summary>
    public class ParsedSourceFile
    {
        public readonly SourceText SourceText;
        public readonly SyntaxTree SyntaxTree;
        public readonly FilePath FilePath;
        public readonly EmbeddedText EmbeddedText;
        public ParseOptions ParseOptions => SyntaxTree.Options;
        public IEnumerable<Diagnostic> Diagnostics => SyntaxTree.GetDiagnostics();

        public ParsedSourceFile(SourceText source, SyntaxTree tree, FilePath filePath)
        {
            SourceText = source;
            FilePath = filePath;
            EmbeddedText = EmbeddedText.FromSource(FilePath, SourceText);
            SyntaxTree = tree;
        }

        public ParsedSourceFile Update(SourceText newText)
            => new ParsedSourceFile(newText, SyntaxTree.WithChangedText(newText), FilePath);

        public ParsedSourceFile UpdateFromFile()
            => Update(SourceText.From(FilePath.ReadAllText(), Encoding.UTF8));

        public static ParsedSourceFile Create(FilePath filePath, CSharpParseOptions options, CancellationToken token = default)
        {
            var newSource = SourceText.From(filePath.ReadAllText(), Encoding.UTF8);
            var newTree = CSharpSyntaxTree.ParseText(newSource, options, filePath, token);
            return new ParsedSourceFile(newSource, newTree, filePath);
        }

        public static ParsedSourceFile CreateFromSource(string source, CSharpParseOptions options, CancellationToken token = default)
        {
            var newSource = SourceText.From(source);
            var filePath = PathUtil.CreateTempFileWithContents(source);
            var newTree = CSharpSyntaxTree.ParseText(newSource, options, filePath, token);
            return new ParsedSourceFile(newSource, newTree, filePath);
        }
    }

    public static partial class RoslynUtils
    {
        public static CSharpParseOptions CSharpLatestParseOptions = new CSharpParseOptions(LanguageVersion.Latest);
        public static CSharpParseOptions CSharpStandardParseOptions = new CSharpParseOptions(LanguageVersion.CSharp7_3);

        public static ParsedSourceFile ParseCSharp(string source, CSharpParseOptions options = default, CancellationToken token = default)
            => ParsedSourceFile.CreateFromSource(source, options, token);

        public static ParsedSourceFile ParseCSharpStandard(string source, CancellationToken token = default)
            => ParsedSourceFile.CreateFromSource(source, CSharpStandardParseOptions, token);

        public static ParsedSourceFile ParseCSharpLatest(string source, CancellationToken token = default)
            => ParsedSourceFile.CreateFromSource(source, CSharpLatestParseOptions, token);

        public static IEnumerable<ParsedSourceFile> ParseCSharpStandard(this IEnumerable<FilePath> files, CancellationToken token )
            => files.Select(f => ParseCSharpStandard(f, token));

        public static IEnumerable<ParsedSourceFile> ParseCSharpLatest(this IEnumerable<FilePath> files, CancellationToken token)
            => files.Select(f => ParseCSharpLatest(f, token));

        public static ParsedSourceFile ParseCSharp(this FilePath filePath, CSharpParseOptions options = null, CancellationToken token = default)
            => ParsedSourceFile.Create(filePath, options ?? CSharpStandardParseOptions, token);

        public static ParsedSourceFile ParseCSharpStandard(this FilePath filePath, CancellationToken token = default)
            => filePath.ParseCSharp(CSharpStandardParseOptions, token);

        public static ParsedSourceFile ParseCSharpLatest(this FilePath filePath, CancellationToken token = default)
            => filePath.ParseCSharp(CSharpLatestParseOptions, token);
    }
}
