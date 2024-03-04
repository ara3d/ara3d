using System;
using System.Collections.Generic;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;
using Ara3D.Parakeet.Grammars;

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

        public enum State
        {
            none,
            ol,
            ul,
            blockquote,
        }

        public HtmlBuilder StateChange(HtmlBuilder builder, ref State oldState, State newState)
        {
            var r = builder;
            if (oldState == newState)
                return r;
            switch (oldState)
            {
                case State.none:
                    break;
                case State.ol:
                    r = r.WriteLine().WriteEndTag("ol").WriteLine();
                    break;
                case State.ul:
                    r = r.WriteLine().WriteEndTag("ul").WriteLine();
                    break;
                case State.blockquote:
                    r = r.WriteLine().WriteEndTag("blockquote").WriteLine();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
            switch (newState)
            {
                case State.none:
                    break; 
                case State.ol:
                    r = r.WriteLine().WriteStartTag("ol").WriteLine();
                    break;
                case State.ul:
                    r = r.WriteLine().WriteStartTag("ul").WriteLine();
                    break;
                case State.blockquote:
                    r = r.WriteLine().WriteStartTag("blockquote").WriteLine();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            oldState = newState;
            return r;
        }

        public HtmlBuilder ToHtml(HtmlBuilder builder = null)
        {
            var state = State.none;
            var r = builder ?? new HtmlBuilder();
            foreach (var child in Children)
            {
                switch (child.Node)
                {
                    case CstDocument doc:
                        r = StateChange(r, ref state, State.none)
                            .WriteStartTag("html")
                            .WriteLine()
                            .WriteBlockTag(child.ToHtml, "body")
                            .WriteLine()
                            .WriteEndTag("html");
                        break;
                    case CstBlankLine _:
                        r = StateChange(r, ref state, State.none)
                            .WriteEmptyTag("br");
                        break;
                    case CstBlock _:
                        r = child.ToHtml(r);
                        break;
                    case CstBlockQuotedLine _:
                        r = StateChange(r, ref state, State.blockquote);
                        r = child.ToHtml(r);
                        break;
                    case CstCodeBlock _:
                        r = r.WriteBlockTag(child.ToHtml, "pre");
                        break;
                    case CstH1Underline _: 
                        break;
                    case CstH2Underline _: 
                        break;
                    case CstHeading _:
                        r = StateChange(r, ref state, State.none);
                        r = child.ToHtml(r);
                        break;
                    case CstHeadingWithOperator h:
                        if (h.Text.StartsWith("######"))
                            r = r.WriteInlineTag(child.ToHtml, "h6").WriteLine();
                        else if (h.Text.StartsWith("#####"))
                            r = r.WriteInlineTag(child.ToHtml, "h5").WriteLine();
                        else if (h.Text.StartsWith("####"))
                            r = r.WriteInlineTag(child.ToHtml, "h4").WriteLine();
                        else if (h.Text.StartsWith("###"))
                            r = r.WriteInlineTag(child.ToHtml, "h3").WriteLine();
                        else if (h.Text.StartsWith("##"))
                            r = r.WriteInlineTag(child.ToHtml, "h2").WriteLine();
                        else if (h.Text.StartsWith("#"))
                            r = r.WriteInlineTag(child.ToHtml, "h1").WriteLine();
                        else
                            throw new Exception("Not recognized heading operator");
                        break;
                    case CstHeadingUnderlined h:
                        if (h.H1Underline.Present)
                            r = r.WriteInlineTag(child.ToHtml, "h1").WriteLine();
                        else if (h.H2Underline.Present)
                            r = r.WriteInlineTag(child.ToHtml, "h2").WriteLine();
                        else
                            throw new Exception("Not a recognized underlined header type");
                        break;
                    case CstIndentedLine _:
                        r = StateChange(r, ref state, State.blockquote);
                        r = child.ToHtml(r);
                        break;
                    case CstLine line:
                        r = r.WriteLine(line.Text);
                        break;
                    case CstUnorderedListItem _:
                        r = StateChange(r, ref state, State.ul)
                            .WriteInlineTag(child.ToHtml, "li")
                            .WriteLine();
                        break;
                    case CstOrderedListItem _:
                        r = StateChange(r, ref state, State.ol)
                            .WriteInlineTag(child.ToHtml, "li")
                            .WriteLine();
                        break;
                    case CstTextLine _:
                        r = child.ToHtml(r);
                        break;;
                }
            }
            return r;
        }
    }

    public class MarkdownParser : Parser<MarkdownBlockGrammar, CstNodeFactory>
    {
        public MarkdownParser(string input)
            : base(input)
        {
            if (!(CstTreeRoot is CstDocument doc))
                throw new Exception("Expected a tree root");
            Content = new MarkdownContent(doc);
        }

        public MarkdownContent Content { get; }
    }
}
