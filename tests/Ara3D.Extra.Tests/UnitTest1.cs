using Ara3D.Parsing.Markdown;
using Ara3D.Utils;

namespace Ara3D.Extra.Tests
{
    public class Tests
    {
        public static DirectoryPath OutputFolder
            => SourceCodeLocation.GetFolder().RelativeFolder("..", "..", "output", "markdown-to-html");

        public static IEnumerable<FilePath> TestMarkdownFiles()
        {
            var rootFolder = SourceCodeLocation.GetFolder().RelativeFolder("..", "..");
            return rootFolder.GetFiles("*.md", true).Where(fp => !fp.Value.Contains("unity"));
        }

        [Test]
        public static void ConvertMarkdownToHtml()
        {
            OutputFolder.CreateAndClearDirectory();
            foreach (var file in TestMarkdownFiles())
                ConvertMarkdownToHtml(file);
        }

        public static void ConvertMarkdownToHtml(FilePath filePath)
        {
            var markdown = filePath.ReadAllText();
            var p = new MarkdownParser(markdown);
            var html = p.Content.ToHtml().ToString();
            var outputFile = filePath.ChangeDirectoryAndExt(OutputFolder, "html");
            outputFile.WriteAllText(html);
        }
    }
}