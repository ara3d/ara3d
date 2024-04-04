using System;
using Ara3D.Logging;
using Ara3D.Utils;

namespace Ara3D.Parsing.Json
{
    public static class JsonParserExtensions
    {
        public static JsonElement ParseJson(this string self, ILogger logger = null)
        {
            var p = new JsonParser(self, logger);
            if (!p.Parser.Succeeded) throw new Exception("Failed to parse JSON");
            return p.Root;
        }

        public static JsonElement ReadAllJson(this FilePath filePath, ILogger logger = null)
            => filePath.ReadAllText().ParseJson(logger);
    }
}