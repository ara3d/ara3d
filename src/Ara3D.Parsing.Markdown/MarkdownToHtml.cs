using System.Collections.Generic;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;
using Ara3D.Parakeet.Grammars;
using Ara3D.Utils;

namespace Ara3D.Parsing.Markdown
{
    public class MarkdownContent
    {
        public List<MarkdownContent> Children { get; }
            = new List<MarkdownContent>();

        public CstNode Node { get; }

        public MarkdownContent(CstNode node)
        {
            Node = node;
            foreach (var child in Node.Children)
            {
                Children.Add(new MarkdownContent(child));
            }
        }

        public CodeBuilder ToHtml()
        {
            var cb = new CodeBuilder();
            foreach (var child in Children)
            {
                switch (child.Node)
                {
                    case CstDocument _: break;
                    case CstBlankLine _: break;
                    case CstBlock _: break;
                    case CstBlockQuotedLine _: break;
                    case CstCodeBlock _: break;
                    case CstH1Underline _: break;
                    case CstH2Underline _: break;
                    case CstHeading _: break;
                    case CstHeadingWithOperator _: break;
                    case CstHeadingUnderlined _: break;
                    case CstIndentedLine _: break;
                    case CstLine _: break;
                    case CstUnorderedListItem _: break;
                    case CstOrderedListItem _: break;
                    case CstTextLine _: break;;


                }
            }
            return cb;
        }
    }

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
                if (cstNode is CstDocument doc)
                    Content = new MarkdownContent(doc);
            }
        }

        public ParserState State { get; }
        public MarkdownContent Content { get; }
    }
}
