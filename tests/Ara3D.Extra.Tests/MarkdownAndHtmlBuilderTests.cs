using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownInlineGrammarNameSpace;
using Ara3D.Parsing.Markdown;
using Ara3D.Utils;

namespace Ara3D.Extra.Tests
{
    public static class MarkdownAndHtmlBuilderTests
    {
        public static void SetUp()
        {
            OutputFolder.Create();
        }

        public static DirectoryPath OutputFolder
            => SourceCodeLocation.GetFolder().RelativeFolder("..", "..", "output", "markdown-to-html");

        public static IEnumerable<FilePath> TestMarkdownFiles()
        {
            var rootFolder = SourceCodeLocation.GetFolder().RelativeFolder("..", "..");
            return rootFolder.GetFiles("*.md", true).Where(fp => !fp.Value.Contains("unity"));
        }

        [TestCaseSource(nameof(TestMarkdownFiles))]
        public static void TestMarkdownToHtml(FilePath filePath)
        {
            var html = ConvertMarkdownToHtml(filePath);
            Console.WriteLine(html);
            var outputFile = filePath.ChangeDirectoryAndExt(OutputFolder, "html");
            outputFile.WriteAllText(html);
        }

        [TestCaseSource(nameof(TestMarkdownFiles))]
        public static void TestMarkdownParserOutput(FilePath filePath)
        {
            var p = new MarkdownBlockParser(filePath.ReadAllText());
            
            Console.WriteLine($"Parser Errors");
            Console.WriteLine(p.Parser.ParserErrorsString);
            
            Console.WriteLine($"Parser Nodes");
            Console.WriteLine(p.Parser.ParserNodesString);

            Console.WriteLine($"Parse XML");
            Console.WriteLine(p.Parser.ParseXml);
        }

        [TestCaseSource(nameof(TestMarkdownFiles))]
        public static void TestLinkedUrls(FilePath filePath)
        {
            foreach (var url in GetAllLinkedUrls(filePath))
                Console.WriteLine(url);
        }

        public static string ConvertMarkdownToHtml(FilePath filePath)
        {
            var markdown = filePath.ReadAllText();
            var p = new MarkdownBlockParser(markdown);
            return p.Document.ToHtml();
        }

        public static IEnumerable<string> GetAllLinkedUrls(FilePath filePath)
        {
            var markdown = filePath.ReadAllText();
            var pBlock = new MarkdownBlockParser(markdown);
            if (!pBlock.Parser.Succeeded) throw new Exception("Failed to parse markdown");
            foreach (var tb in pBlock.Document.GetAllTextBlocks())
            {
                var pInline = tb.ParseInlineMarkdown();
                foreach (var url in pInline.Parser.Cst.Descendants().OfType<CstUrl>())
                    yield return url.Text;

                foreach (var url in pInline.Parser.Cst.Descendants().OfType<CstPlainTextUrl>())
                    yield return url.Text;
            }
        }
    }
}