﻿using System.Linq;

namespace Parakeet
{
    public class CstXmlBuilder : CodeBuilder<CstXmlBuilder>
    {
        public CstXmlBuilder Write(CstNode node)
        {
            var type = node.GetType().Name;
            if (node.IsLeaf)
            {
                return WriteLine($"<{type}>{node.Text}</{type}>");
            }
            var r = WriteLine($"<{type}>").Indent();
            r = node.Children.Aggregate(r, (current, c) => current.Write(c));
            return r.Dedent().WriteLine($"</{type}>");
        }
    }
}
