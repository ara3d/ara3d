using System;
using System.Collections.Generic;
using Ara3D.Parakeet;
using Ara3D.Parakeet.Cst.MarkdownBlockGrammarNameSpace;
using Ara3D.Parakeet.Grammars;

namespace Ara3D.Parsing.Markdown
{
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
