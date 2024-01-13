using Parakeet;
using Parakeet.CST;

namespace Ara3D.Parsing.CST
{
    public class CstLocation : ILocation
    {
        public CstNode Node { get; }
        public CstLocation(CstNode node)
            => Node = node;
    }
}