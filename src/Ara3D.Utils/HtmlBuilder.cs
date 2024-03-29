﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ara3D.Utils
{
    public class HtmlBuilder : CodeBuilder<HtmlBuilder>
    {
        public HtmlBuilder WriteBlockTag(Func<HtmlBuilder, HtmlBuilder> func, string tag, params HtmlAttribute[] attributes)
            => func(WriteStartTag(tag, attributes).WriteLine().Indent()).Dedent().WriteLine().WriteEndTag(tag).WriteLine();

        public HtmlBuilder WriteInlineTag(Func<HtmlBuilder, HtmlBuilder> func, string tag, params HtmlAttribute[] attributes)
            => func(WriteStartTag(tag, attributes)).WriteEndTag(tag);

        public HtmlBuilder Write(IEnumerable<HtmlAttribute> attributes)
            => attributes.Aggregate(this, (current, attr) => current.Write($" {attr}"));
        
        public HtmlBuilder WriteStartTag(string tagName, params HtmlAttribute[] attributes)
            => Write($"<{tagName}").Write(attributes).Write(">");

        public HtmlBuilder WriteEndTag(string tagName)
            => Write($"</{tagName}>");

        public HtmlBuilder WriteEmptyTag(string tagName, params HtmlAttribute[] attributes)
            => Write($"<{tagName}").Write(attributes).Write("/>");

        public HtmlBuilder WriteEscaped(string text)
            => Write(text.EscapeCommonHtmlEntities());

        public HtmlBuilder WriteEscapedLine(string text)
            => WriteEscaped(text).WriteLine();

        public HtmlBuilder WriteTaggedText(string tag, string text)
            => WriteStartTag(tag).WriteEscaped(text).WriteEndTag(tag);
    }
}