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

        public static object Convert(this JsonElement self, Type type)
        {
            if (type == typeof(int))
                return self.AsInteger;
            if (type == typeof(float))
                return self.AsFloat;
            if (type == typeof(string))
                return self.AsString;
            if (type == typeof(bool))
                return self.AsBool;
            if (type == typeof(double))
                return self.AsDouble;

            if (type.IsClass)
            {
                if (self is JsonObject o)
                {
                    var r = Activator.CreateInstance(type);
                    foreach (var field in o.Fields)
                    {
                        var fi = type.GetField(field.Key);
                        if (fi != null)
                        {
                            var val = Convert(field.Value, fi.FieldType);
                            fi.SetValue(r, val);
                        }
                        else
                        {
                            var pi = type.GetProperty(field.Key);
                            if (pi != null && pi.CanWrite)
                            {
                                var val = Convert(field.Value, pi.PropertyType);
                                pi.SetValue(r, val);
                            }
                        }
                    }

                    return r;
                }
            }

            throw new NotSupportedException("Not a supported cast");
        }

        public static T Convert<T>(this JsonElement e)
            => (T)e.Convert(typeof(T));
    }
}