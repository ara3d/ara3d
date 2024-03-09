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
        public MdList(bool ordered, params MdListItem[] items) : base(items)
            => (Ordered, Items) = (ordered, items);
        public IReadOnlyList<MdListItem> Items { get; }
        public bool Ordered { get; }
    }

    public class MdUnorderedList : MdList
    {
        public MdUnorderedList(params MdListItem[] items) : base(false, items) { }
    }

    public class MdOrderedList : MdList
    {
        public MdOrderedList(params MdListItem[] items) : base(true, items) { }
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
        public readonly string Text;
        public MdCodeBlock(string text)
            => Text = text;
    }

    public class MdListItem : MdBlock
    {
        public MdListItem(params MdBlock[] children)
            : base(children)
        { }
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