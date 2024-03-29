using System.Collections.Generic;
using System.Linq;

namespace Ara3D.Parsing.Markdown
{
    public static class MdBlockExtensions
    {
        public static IEnumerable<MdBlock> GetAllDescendants(this MdBlock block)
        {
            yield return block;
            foreach (var c in block.Children)
            foreach (var c2 in c.GetAllDescendants())
                yield return c2;
        }

        public static IEnumerable<MdText> GetAllTextBlocks(this MdBlock block)
            => block.GetAllDescendants().OfType<MdText>();

        public static MarkdownInlineParser ParseInlineMarkdown(this MdText text)
        {

        }
    }
}