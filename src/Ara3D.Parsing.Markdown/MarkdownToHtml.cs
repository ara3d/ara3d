using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;
using Ara3D.Parakeet.Grammars;

namespace Ara3D.Parsing.Markdown
{
    public class MarkdownBlock
    { }

    public class Markdown
    {
        public Markdown(string input)
        {
            State = MarkdownBlockGrammar.Instance.Parse(input);
            var node = State?.Node;
            var tree = node?.ToParseTree();
            if (tree != null)
            {
                var factory = new CstNodeFactory();
                var cstNode = factory.Create(tree);
                Document = (CstDocument)cstNode;
            }
        }

        public ParserState State { get; }
        public CstDocument Document { get; }
    }

    public class MarkdownToHtml
    {
        // TODO: 
    }
}
