using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;

namespace Ara3D.Parsing.Markdown
{
    public static class MdBlockFactory
    {
        public static MdBlock[] ToMdBlocks(this IEnumerable<CstNode> nodes)
            => nodes.Select(ToMdBlock).ToArray();

        public static MdText ToMdText(this CstTextLine textLine)
            => new MdText(textLine.Text);

        public static int HeadingOperatorToLevel(CstHeadingOperator op)
            => op.Text.Trim().Length;

        public static MdBlock ToMdBlock(this CstNode node)
        {
            if (node == null)
                return null;

            switch (node)
            {
                case CstDocument doc:
                    return new MdDocument(doc.Children.ToMdBlocks());
                
                case CstBlankLine _:
                    return new MdBr();
                
                case CstBlock block:
                    return block.Line.Present ? block.Line.ToMdBlock()
                        : block.Comment.Present ? block.Comment.ToMdBlock()
                        : block.CodeBlock.Present ? block.CodeBlock.ToMdBlock()
                        : null;

                case CstBlockQuotedLine bq:
                    return new MdQuote(bq.Line.ToMdBlock());

                case CstCodeBlock cb:
                    return new MdCodeBlock(cb.Text);

                case CstH1Underline h1:
                    throw new Exception($"Unexpected node {node}");

                case CstH2Underline h2:
                    throw new Exception($"Unexpected node {node}");

                case CstHeading h:
                    return 
                        h.HeadingUnderlined.Present ? h.HeadingUnderlined.ToMdBlock() : 
                        h.HeadingWithOperator.Present ? h.HeadingWithOperator.ToMdBlock() : 
                        throw new Exception($"Expected heading");

                case CstHeadingWithOperator h:
                    return new MdHeader(
                        HeadingOperatorToLevel(
                            h.HeadingOperator.Node),
                            new MdText(h.Line.Text));

                case CstHeadingUnderlined h:
                    return 
                        h.H1Underline.Present ? new MdHeader(1, h.TextLine.Node.ToMdText()) : 
                        h.H2Underline.Present ? new MdHeader(2, h.TextLine.Node.ToMdText()) : 
                        throw new Exception($"Expected heading");
                
                case CstLine line:
                    break;
                
                case CstUnorderedListItem _:
                    break;
                
                case CstOrderedListItem _:
                    break;
                
                case CstTextLine _:
                    break; ;
            }

            throw new NotImplementedException($"Not handled node type {node}");
        }
    }
}