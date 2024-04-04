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

        public static MdText ToMdText(this CstTextLine textLine, bool trim)
            => new MdText(trim ? textLine.Text.Trim() : textLine.Text);

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
                    return block.Line.Present ? block.Line.Node.ToMdBlock()
                        : block.Comment.Present ? block.Comment.Node.ToMdBlock()
                        : block.CodeBlock.Present ? block.CodeBlock.Node.ToMdBlock()
                        : null;

                case CstBlockQuotedLine bq:
                    return new MdQuote(bq.RestOfLine.Node.ToMdBlock());

                case CstRestOfLine restOfLine:
                    if (restOfLine.Children.Count > 1)
                        throw new Exception("Internal error expected single line");
                    return restOfLine.Line.Node.ToMdBlock();

                case CstCodeBlock cb:
                    return new MdCodeBlock(cb.CodeBlockLang.Node?.Text ?? "", 
                        cb.CodeBlockText.Node.Text);

                case CstH1Underline h1:
                    throw new Exception($"Unexpected node {node}");

                case CstH2Underline h2:
                    throw new Exception($"Unexpected node {node}");

                case CstHeading h:
                    return 
                        h.HeadingUnderlined.Present ? h.HeadingUnderlined.Node.ToMdBlock() : 
                        h.HeadingWithOperator.Present ? h.HeadingWithOperator.Node.ToMdBlock() : 
                        throw new Exception($"Expected heading");

                case CstHeadingWithOperator h:
                    return new MdHeader(
                        HeadingOperatorToLevel(
                            h.HeadingOperator.Node),
                            h.TextLine.Node.ToMdText(true));

                case CstHeadingUnderlined h:
                    return 
                        h.H1Underline.Present ? new MdHeader(1, h.TextLine.Node.ToMdText(true)) : 
                        h.H2Underline.Present ? new MdHeader(2, h.TextLine.Node.ToMdText(true)) : 
                        throw new Exception($"Expected heading");
                
                case CstLine line:
                    return line.Node.ToMdBlock();
                
                case CstUnorderedListItem uli:
                    return new MdListItem(uli.Indents.Count, false, uli.TextLine.Node.ToMdBlock());
                
                case CstOrderedListItem oli:
                    return new MdListItem(oli.Indents.Count, true, oli.TextLine.Node.ToMdBlock());

                case CstTextLine tl:
                    return new MdText(tl.Text);

                case CstNonEmptyTextLine nonEmptyText:
                    return new MdText(nonEmptyText.Text);

                case CstComment cstComment:
                    // For now just return empty text. 
                    return new MdText("");
            }

            throw new NotImplementedException($"Not handled node type {node}");
        }
    }
}