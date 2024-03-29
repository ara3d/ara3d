using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Logging;
using Ara3D.Parakeet;
using Ara3D.Utils;

namespace Ara3D.Parsing.Json
{
    public class JsonParser
    {
        public Parser Parser { get; }
        public JsonElement Root { get; }

        public JsonParser(string input, ILogger logger = null)
        {
            Parser = CommonParsers.JsonParser(input, logger);
            if (!Parser.Succeeded) return;
            Root = Parser.ParseTree.ToElement();
        }
    }

    public static class JsonElementFactory
    {
        public static JsonElement ParseConstant(string input)
            => input == "true" ? JsonTrue.Instance
                : input == "false" ? JsonFalse.Instance
                : input == "null" ? (JsonElement)JsonNull.Instance
                : throw new Exception($"Not a recognized constant {input}");

        public static JsonField ToField(this IReadOnlyList<ParserTreeNode> children)
            => children.Count != 2
                ? throw new Exception($"Expected 2 children not {children.Count}")
                : JsonField.Create(
                    (JsonString)ToElement(children[0]), 
                    ToElement(children[1]));

        public static JsonElement ToElement(this ParserTreeNode node)
        {
            if (node == null) return null;
            switch (node.Type)
            {
                case "Array": return JsonArray.Create(node.Children.Select(ToElement));
                case "Constant": return ParseConstant(node.Contents);
                case "Json": return node.Children.Single().ToElement();
                case "Element": return node.Children.Single().ToElement();
                case "Member": return node.Children.ToField();
                case "Number": return JsonNumber.Create(node.Contents);
                case "Object": return JsonObject.Create(node.Children.Select(c => (JsonField)c.ToElement()));
                case "String": return (JsonString)node.Contents.StripQuotes();
                default: throw new Exception($"Unrecognized parse node {node.Type}");
            }

        }
    }
}
