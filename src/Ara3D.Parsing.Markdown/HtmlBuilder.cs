using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Utils;

namespace Ara3D.Parsing.Markdown
{
    public class HtmlAttribute
    {
        public readonly string Name;
        public readonly string Value;

        public HtmlAttribute(string name, string value)
            => (Name,Value) = (name,value.Trim());

        public static implicit operator (string, string)(HtmlAttribute attr)
            => (attr.Name, attr.Value);

        public static implicit operator HtmlAttribute((string, string) nameValueTuple)
            => new HtmlAttribute(nameValueTuple.Item1, nameValueTuple.Item2);

        public override string ToString()
            => HtmlExtensions.ToHtmlAttribute(Name, Value);
    }

    public static class HtmlExtensions
    {
        public static string EscapeCommonHtmlEntities(this string html)
            => html.Replace("<", "&lt;").Replace("&", "&amp;");

        public static string EscapeAttributeValueText(this string html)
            => html.EscapeCommonHtmlEntities().Replace("\"", "&quot;").Replace("\'", "&apos;");

        public static string ToHtmlAttribute(string name, string value)
            => $"{name} = '{value.EscapeAttributeValueText()}'";
    }

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