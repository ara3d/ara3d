﻿using System.Diagnostics;
using Parakeet.Demos;

namespace Parakeet.Tests
{
    public static class PlatoTests
    {
        public static PlatoGrammar Grammar = PlatoGrammar.Instance;

        [Test]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\libraries.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\types.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\concepts.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\intrinsics.plato")]
        public static void TestFile(string file)
        {
            var pi = ParserInput.FromFile(file);
            ParserTests.ParseTest(pi, Grammar.File);
        }

        [Test]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\libraries.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\types.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\concepts.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\intrinsics.plato")]
        public static void TestTokenizer(string file)
        {
            var pi = ParserInput.FromFile(file);
            ParserTests.ParseTest(pi, PlatoTokenGrammar.Instance.Tokenizer);
        }

        [Test]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\libraries.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\types.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\concepts.plato")]
        [TestCase("C:\\Users\\cdigg\\git\\plato\\PlatoStandardLibrary\\intrinsics.plato")]
        public static void OutputTokens(string file)
        {
            var pi = ParserInput.FromFile(file);
            var state = pi.Parse(PlatoTokenGrammar.Instance.Tokenizer);
            Debug.Assert(state != null);
            var nodes = state.AllNodes().ToList();
            foreach (var n in nodes)
            {
                Console.WriteLine($"{n.Name} {n.Start}+{n.Length}");
            }
        }


        public static string[] Literals =
        {
            "42",
            "\"Hello world\"",
            "'q'",
            "true",
            "false",
            "3.14e-10",
            "-3.14E+10",
            "0.001",
        };

        public static string[] Identifiers =
        {
            "a",
            "abc",
            "ab_cd",
        };

        public static string[] Expressions =
        {
            "f()",
            "abc(123)",
            "f(g(), h())",
        };

        public static string[] Statements =
        {
            "a = x",
            "let x = 12",
            "break",
            "continue",
            "return x",
            "if x then a else b",
            "_",
            "x",
            "f()",
            "f(g(h(123)))",
            "{}",
            "{ }",
            "{ {}; {}; }",
            "{ break; }",
            "{ break; continue; let x = _; { }; return x; f(); }",
            "if true then { } else { }",
            "while true do { }",
            "while false do f()",
            "if if x then y else z then a else b",
            "while x do { a; b(); x = 12; }",
        };

        [Test]
        [TestCaseSource(nameof(Identifiers))]
        [TestCaseSource(nameof(Literals))]
        [TestCaseSource(nameof(Expressions))]
        public static void TestExpressions(string input)
            => ParserTests.SingleParseTest(input, Grammar.Expression);


        [Test]
        [TestCase("a", "Identifier")]
        [TestCase("_", "Identifier")]
        [TestCase("_1", "Identifier")]
        [TestCase("abc_123", "Identifier")]
        [TestCase("type F", "DeclaredType")]
        [TestCase("concept my_concept", "DeclaredType")]
        [TestCase(" ", "CommentOrSpaces")]
        [TestCase("", "CommentOrSpaces")]
        [TestCase("/* Abc */", "Comment")]
        [TestCase("/* Abc */", "CommentOrSpaces")]
        [TestCase("/* Abc */ /* 123 */", "CommentOrSpaces")]
        [TestCase("// Hello\n", "Comment")]
        [TestCase("{ x:T", "DeclaredField")]
        [TestCase("; x:T", "DeclaredField")]
        [TestCase("{ xyz : Type1", "DeclaredField")]
        [TestCase("; xyz(a: T):T", "DeclaredFunction")]
        public static void TargetedTest(string input, string name)
        {   
            var rule = PlatoTokenGrammar.Instance.GetRuleFromName(name);
            var result = ParserTests.ParseTest(input, rule);
            Assert.IsTrue(result == 1);
        }
    }
}