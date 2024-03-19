using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownInlineGrammarNameSpace;
using Ara3D.Utils;
using CstIdentifier = Ara3D.Parakeet.Cst.MarkdownInlineGrammarNameSpace.CstIdentifier;

namespace Ara3D.Parsing.Markdown
{
    public static class HtmlFactory
    {
        public static HtmlBuilder WriteChildren(this HtmlBuilder bldr, IEnumerable<MdBlock> children, string tag)
            => children.Aggregate(bldr.WriteStartTag(tag).WriteLine(), (current, x) => current.Write(x))
                .WriteEndTag(tag).WriteLine();

        public static string GetTag(this MdList list)
            => list.Ordered ? "ol" : "ul";

        public static HtmlAttribute[] ToAttributes(params (string, string)[] namesValueTuples)
        {
            var r = new List<HtmlAttribute>();
            foreach (var tuple in namesValueTuples)
                if (!tuple.Item2.IsNullOrWhiteSpace())
                    r.Add(tuple);
            return r.ToArray();
        }

        public static HtmlBuilder Write(this HtmlBuilder bldr, IEnumerable<CstNode> nodes)
            => nodes.Aggregate(bldr, (b, n) => b.WriteGenericNode(n));

        public static HtmlBuilder WriteGenericNode(this HtmlBuilder bldr, CstNode node)
        {
            if (node is IMarkdownInlineCstNode inlineCstNode)
                return bldr.Write(inlineCstNode);
            throw new NotImplementedException();
        }

        public static HtmlBuilder Write(this HtmlBuilder bldr, IMarkdownInlineCstNode node)
        {
            switch (node)
            {
                case null:
                    return bldr;

                case CstBold cstBold:
                    return bldr.WriteInlineTag(b => b.Write(cstBold.InnerText.Node), "b");

                case CstBoldAndItalic cstBoldAndItalic:
                    return bldr.WriteStartTag("i").WriteInlineTag(b => b.Write(cstBoldAndItalic.InnerText.Node), "i").WriteEndTag("b");

                case CstCode cstCode:
                    return bldr.WriteInlineTag(b => b.Write(cstCode.InnerText.Node), "code");

                case CstContent cstContent:
                    return bldr.Write(cstContent.Children);

                case CstEmail cstEmail:
                    throw new NotImplementedException();

                case CstEmailLink cstEmailLink:
                    return bldr.WriteStartTag("a", ("href", $"mailto:{cstEmailLink.Email.Node?.Text ?? ""}"))
                        .Write(cstEmailLink.Email.Node?.Text ?? "")
                        .WriteEndTag("a");
                
                case CstEscapedChar cstEscapedChar:
                    return bldr.Write(cstEscapedChar.Text.Substring(1));
                
                case CstHtmlTag cstHtmlTag:
                    return bldr.Write(cstHtmlTag.Text);

                case CstIdentifier cstIdentifier:
                    throw new NotImplementedException();

                case CstImg cstImg:
                    return bldr.WriteEmptyTag("img", ToAttributes(
                        ("alt", cstImg.AltText.Node?.Text), 
                        ("src", cstImg.Url.Node?.Text),
                        ("title", cstImg.UrlTitle.Node?.Text)));

                case CstInnerText cstInnerText:
                    return bldr.Write(cstInnerText.Children);

                case CstItalic cstItalic:
                    return bldr.WriteInlineTag(b => b.Write(cstItalic.InnerText.Node), "i");

                case CstLink cstLink:
                    return bldr.WriteStartTag("a", ToAttributes(
                        ("href", cstLink.Url.Node?.Text),
                        ("title", cstLink.UrlTitle.Node?.Text)))
                        .Write(cstLink.LinkedText.Nodes)
                        .WriteEndTag("a");

                case CstPlainText cstPlainText:
                    return bldr.WriteEscaped(cstPlainText.Text);

                case CstPlainTextUrl cstPlainTextUrl:
                    return bldr.WriteStartTag("a", ("href", cstPlainTextUrl.Text))
                        .Write(cstPlainTextUrl.Text).WriteEndTag("a");

                case CstStrikethrough cstStrikethrough:
                    return bldr.WriteInlineTag(b => b.Write(cstStrikethrough.InnerText.Node), "strike");

                case CstUrl cstUrl:
                    throw new NotImplementedException();

                case CstUrlLink cstUrlLink:
                    return bldr.WriteStartTag("a", ToAttributes(("href", cstUrlLink.Url.Node?.Text)))
                        .Write(cstUrlLink.Url.Node?.Text).WriteEndTag("a");

                case CstUrlTitle cstUrlTitle:
                    throw new NotImplementedException();

                case CstLinkedText linkedText:
                    return bldr.Write(linkedText.Children);

                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }

            // Note: not reachable 
            throw new NotImplementedException();
        }

        public static HtmlBuilder WriteInlineMarkdown(this HtmlBuilder bldr, string input)
        {
            if (input.IsNullOrWhiteSpace())
                return bldr.Write(input);

            var p = new MarkdownInlineParser(input);
            if (!p.Parser.Succeeded)
                throw new Exception("Failed to parse markdown content");

            if (!(p.Parser.Cst is CstContent content))
                throw new Exception("Failed to create CST content node");

            return bldr.Write(content);
        }

        public static HtmlBuilder Write(this HtmlBuilder bldr, MdBlock block)
        {
            var r = bldr ?? new HtmlBuilder();
            switch (block)
            {
                case MdBr _:
                    return r.WriteEmptyTag("br").WriteLine();

                case MdCodeBlock mdCodeBlock:
                    return r.WriteBlockTag(b => b.WriteEscapedLine(mdCodeBlock.Text), 
                        "pre", ToAttributes(("lang", mdCodeBlock.Lang)));

                case MdDocument mdDocument:
                    return r.WriteLine("<html>").WriteChildren(mdDocument.Children, "body").WriteLine("</html>");

                case MdHeader mdHeader:
                    return r.WriteInlineTag(b => b.Write(mdHeader.Content), $"h{mdHeader.Level}");

                case MdList mdList:
                    return r.WriteChildren(mdList.Items, mdList.GetTag());

                case MdListItem mdListItem:
                    return r.WriteChildren(mdListItem.Children, "li");

                case MdParagraph mdParagraph:
                    return r.WriteChildren(mdParagraph.Children, "p");
                
                case MdQuote mdQuote:
                    return r.WriteChildren(mdQuote.Children, "blockquote");
                
                case MdText mdText:
                    return r.WriteInlineMarkdown(mdText.Text);

                default:
                    throw new ArgumentOutOfRangeException(nameof(block));
            }

            return r;
        }

        public static string ToHtml(this MdBlock block)
            => (new HtmlBuilder()).Write(block).ToString();
    }
}