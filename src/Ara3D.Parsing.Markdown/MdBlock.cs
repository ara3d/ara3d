using System.Collections.Generic;

namespace Ara3D.Parsing.Markdown
{
    public class MdBlock
    {
        public IReadOnlyList<MdBlock> Children { get; }
        public MdBlock(params MdBlock[] children)
            => Children = children;
    }

    public class MdDocument : MdBlock
    {
        public MdDocument(params MdBlock[] children)
            : base(children)
        { }
    }

    public class MdList : MdBlock
    {
        public MdList(int nesting, bool ordered, params MdListItem[] items) : base(items)
            => (Nesting, Ordered, Items) = (nesting, ordered, items);
        public IReadOnlyList<MdListItem> Items { get; }
        public bool Ordered { get; }
        public int Nesting { get; }
    }

    public class MdQuote : MdBlock
    {
        public MdQuote(params MdBlock[] children)
            : base(children)
        { }
    }

    public class MdParagraph : MdBlock
    {
        public MdParagraph(params MdBlock[] children)
            : base(children)
        { }
    }

    public class MdCodeBlock : MdBlock
    {
        public readonly string Lang;
        public readonly string Text;
        public MdCodeBlock(string lang, string text)
            => (Lang, Text) = (lang, text);
    }

    public class MdListItem : MdBlock
    {
        public MdListItem(int nesting, bool ordered, params MdBlock[] children)
            : base(children)
            => (Nesting, Ordered) = (nesting, ordered);

        public bool Ordered { get; }
        public int Nesting { get; }
    }

    public class MdBr : MdBlock
    { }

    public class MdText : MdBlock
    {
        public string Text { get; }
        public MdText(string text)
            => Text = text;
    }

    public class MdHeader : MdBlock
    {
        public int Level { get; }
        public MdText Content { get; }
        public MdHeader(int level, MdText content)
            : base(content) => (Level, Content) = (level, content);
    }
}