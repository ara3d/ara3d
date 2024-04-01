using Ara3D.Logging;
using Ara3D.Parakeet;

namespace Ara3D.Parsing.Markdown
{
    public class MarkdownInlineParser
    {
        public MarkdownInlineParser(string input, ILogger logger = null)
        {
            Input = input;
            Parser = CommonParsers.MarkdownInlineParser(Input, logger);
        }

        public ParserInput Input { get; }
        public Parser Parser { get; }
    }
}