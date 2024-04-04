using Ara3D.Logging;

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
}
