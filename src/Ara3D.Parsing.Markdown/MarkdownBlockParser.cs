using System;
using Ara3D.Logging;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;

namespace Ara3D.Parsing.Markdown
{
    public class MarkdownBlockParser
    {
        public MarkdownBlockParser(string input, ILogger logger = null)
        {
            Input = input;
            Parser = CommonParsers.MarkdownBlockParser(Input, logger);
            if (!(Parser.Cst is CstDocument doc))
                throw new Exception("Expected a tree root");
            Document = (MdDocument)doc.ToMdBlock();
        }

        public ParserInput Input { get; }
        public Parser Parser { get; }
        public MdDocument Document { get; }
    }

    public class MarkdownInlineParser
    {
        public MarkdownInlineParser(string input, ILogger logger = null)
        {
            Input = input;
            Parser = CommonParsers.MarkdownInlineParser(Input, logger);
        }

        public ParserInput Input { get; }
        public Parser Parser { get; }
        public CstNode Cst => Parser.Cst;
    }
}
