using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Parakeet;

namespace Ara3D.Parsing
{
    public class Parser<TGrammar, TNodeFactory>
        where TGrammar : IGrammar, new()
        where TNodeFactory: INodeFactory, new()
    {
        public TGrammar Grammar { get; }
        public TNodeFactory NodeFactory { get; }
        public ParserInput Input { get; }
        public ParserState Result { get; }
        public bool Succeeded => Result != null && Result.LastError == null && Result.AtEnd();
        public ParserTreeNode ParseTreeRoot { get; } 
        public CstNode CstTreeRoot { get; }
        public List<string> ErrorMessages { get; } = new List<string>();

        public Parser(string input)
        {
            Grammar = new TGrammar();
            NodeFactory = new TNodeFactory();
            Input = new ParserInput(input);

            try
            {
                Result = Grammar.StartRule.Parse(Input);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(e.Message);
            }

            if (Result == null)
                ErrorMessages.Add("Parse failed without a result");

            if (Result?.LastError != null)
                ErrorMessages.AddRange(Result.AllErrors().Select(e => e.Message));

            ParseTreeRoot = Result?.Node?.ToParseTree();

            CstTreeRoot = ParseTreeRoot != null 
                ? NodeFactory.Create(ParseTreeRoot) 
                : null;
        }

        public string ParseTreeXml 
            => ParseTreeRoot?.ToXml() ?? "";

        public string CstTreeXml
            => CstTreeRoot?.ToXml().ToString();
    }
}
    