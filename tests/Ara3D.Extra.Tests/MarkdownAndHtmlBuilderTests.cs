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

        public static string ConvertMarkdownToHtml(FilePath filePath)
        {
            var markdown = filePath.ReadAllText();
            var p = new MarkdownParser(markdown);
            return p.Document.ToHtml().ToString();
        }
    }
}