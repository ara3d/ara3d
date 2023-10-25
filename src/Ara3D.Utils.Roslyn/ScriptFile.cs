using System.IO;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Ara3D.Utils.Roslyn
{
    public class ScriptFile
    {
        public readonly EmbeddedText EmbeddedText;
        public readonly SourceText SourceText;
        public readonly SyntaxTree SyntaxTree;
        public readonly string FilePath;

        public ScriptFile(string filePath, SourceText source, SyntaxTree tree)
        {
            FilePath = filePath;
            SourceText = source;
            EmbeddedText = EmbeddedText.FromSource(FilePath, SourceText);
            SyntaxTree = tree;
        }

        public static ScriptFile Create(string filePath, CSharpParseOptions options, CancellationToken token = default)
        {
            var newSource = SourceText.From(File.ReadAllText(filePath), Encoding.UTF8);
            var newTree = CSharpSyntaxTree.ParseText(newSource, options, filePath, token);
            return new ScriptFile(filePath, newSource, newTree);
        }
    }
}
