using System;
using Ara3D.Utils;

namespace Ara3D.Parsing.Json
{
    public static class JsonExtensions
    {
        public static JsonElement AsJson(this string self)
            => new JsonParser(self).Root;

        public static JsonStringBuilder BuildString(this JsonElement self, JsonStringBuilder bldr = null)
        {
            bldr = bldr ?? new JsonStringBuilder();
            switch (self)
            {
                case JsonArray jsonArray:
                    return bldr.WriteStartBlock("[")
                        .WriteList(jsonArray.Elements, 
                            (sb, e) => e.BuildString(sb), 
                            (sb) => sb.WriteLine(","))
                        .WriteEndBlock("]");

                case JsonObject jsonObject:
                    return bldr.WriteStartBlock()
                        .WriteList(jsonObject.Fields, 
                            (sb, e) => e.BuildString(sb),
                            (sb) => sb.WriteLine(", "))
                        .WriteEndBlock();
            }
            return bldr.Write(self.ToString());
        }
    }
}