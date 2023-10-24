﻿namespace Parakeet.Tests
{
    public static class TreeTests
    {
        [Test]
        public static void TestFunctionParse()
        {
            var input = "double Radians(double x) => x / 360;";
            var rule = CSharpTests.Grammar.MethodDeclaration;
            var pi = new ParserInput(input);
            var ps = pi.Parse(rule);
            Assert.IsTrue(ps.AtEnd());
            Console.WriteLine("Nodes:");
            foreach (var node in ps.AllNodes())   
            {
                Console.WriteLine(node);
            }
            Console.WriteLine("Tree");
            var tree = ps.Node.ToParseTree();
            Console.WriteLine(tree.BuildXmlString());
        }
    }
}
