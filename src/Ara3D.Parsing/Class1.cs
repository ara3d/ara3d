using System;

namespace Ara3D.Parsing
{
    public class Parser<TGrammar>
        where TGrammar : new()
    {
        public static TGrammar Grammar { get; private set; }

        public Parser()
        {
            if (Grammar == null)
                Grammar = new TGrammar();
        }
    }
}
