using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Utils;

namespace Ara3D.Parsing.Json
{
    public abstract class JsonElement
    {
        public static JsonElement CreateElement(object x)
        {
            switch (x)
            {
                case null: return JsonNull.Instance;
                case bool b: return b ? (JsonElement)JsonTrue.Instance : JsonFalse.Instance;
                case char c: return (JsonString)c.ToString();
                case string s: return (JsonString)s;
                case decimal d: return (JsonNumber)decimal.ToDouble(d);
            }

            // If we can cast the value to a double, then we encode it as a number
            if (x.CanCastToDouble())
                return (JsonNumber)x.CastToDouble();

            // If we can find a GetEnumerator function, then we can treat it as an array. 
            var e = x.GetEnumerator();
            if (e != null)
                return JsonArray.Create(e);

            // By default we create the underlying object as a dictionary using its public fields and
            // auto-backed properties. 
            return JsonObject.Create(x);
        }

        public abstract IEnumerable<JsonElement> GetChildren();
    }

    public class JsonField : JsonElement
    {
        public JsonField(JsonString key, JsonElement value) => (Key, Value) = (key, value); 
        public JsonString Key { get; }
        public JsonElement Value { get; }
        public override string ToString() => $"{Key}:{Value}";
        public static JsonField Create(JsonString key, JsonElement value) => (key, value);
        public static JsonField Create(KeyValuePair<string, object> keyValue) => Create(keyValue.Key, CreateElement(keyValue.Value));
        public static implicit operator (JsonString, JsonElement)(JsonField field) => (field.Key, field.Value);
        public static implicit operator JsonField((JsonString, JsonElement) tuple) => new JsonField(tuple.Item1, tuple.Item2);
        public override IEnumerable<JsonElement> GetChildren() => new[] { Key, Value };
    }

    public class JsonObject : JsonElement
    {
        public JsonObject(IReadOnlyList<JsonField> fields) => Fields = fields;
        public IReadOnlyList<JsonField> Fields { get; }
        public override string ToString() => "{" + Fields.JoinStringsWithComma() + "}";
        public static JsonObject Create(params JsonField[] fields) => new JsonObject(fields);
        public static JsonObject Create(IEnumerable<JsonField> fields) => Create(fields.ToArray());
        public static JsonObject Create(IEnumerable<KeyValuePair<string, object>> keysAndValues) => Create(keysAndValues.Select(JsonField.Create));
        public static JsonObject Create(object o) => Create(o.PublicFieldsAndFieldBackedPropertiesToDictionary());
        public override IEnumerable<JsonElement> GetChildren() => Fields;
    }

    public class JsonArray : JsonElement
    {
        public JsonArray(IReadOnlyList<JsonElement> elements)
            => Elements = elements;
        public IReadOnlyList<JsonElement> Elements { get; }
        public override string ToString() => "[" + Elements.JoinStringsWithComma() + "]";
        public static JsonArray Create(IEnumerable<JsonElement> elements) => new JsonArray(elements.ToList());
        public static JsonArray Create(params JsonElement[] elements) => new JsonArray(elements);
        public static JsonArray Create(IEnumerator enumerator) => Create(enumerator.ToList(CreateElement));
        public static implicit operator JsonElement[](JsonArray array) => array.Elements.ToArray();
        public static implicit operator JsonArray(JsonElement[] array) => new JsonArray(array);
        public override IEnumerable<JsonElement> GetChildren() => Elements;
    }

    public class JsonString : JsonElement
    {
        public JsonString(string value)
            => Value = value;
        public string Value { get; }
        public static readonly JsonString Empty = new JsonString(string.Empty);
        public static readonly JsonString Zero = new JsonString("0");
        public static readonly JsonString One = new JsonString("1");
        public static readonly JsonString True = new JsonString("true");
        public static readonly JsonString False = new JsonString("false");
        public static readonly JsonString Null = new JsonString("null");
        public override string ToString() => $"\"{Value}\"";
        public static implicit operator string(JsonString value) => value.Value;
        public static implicit operator JsonString(string value) => new JsonString(value);
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }

    public class JsonNumber : JsonElement
    {
        public JsonNumber(double value)
            => Value = value;
        public double Value { get; }
        public static readonly JsonNumber Zero = new JsonNumber(0);
        public static readonly JsonNumber One = new JsonNumber(1);
        public override string ToString() => Value.ToString();
        public static JsonNumber Create(string value) => double.Parse(value);
        public static implicit operator double(JsonNumber value) => value.Value;
        public static implicit operator JsonNumber(double value) => new JsonNumber(value);
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }

    public class JsonTrue : JsonElement
    {
        public static readonly JsonTrue Instance = new JsonTrue();
        public override string ToString() => "true";
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }

    public class JsonFalse : JsonElement
    {
        public static readonly JsonFalse Instance = new JsonFalse();
        public override string ToString() => "false";
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }

    public class JsonNull : JsonElement
    {
        public static readonly JsonNull Instance = new JsonNull();
        public override string ToString() => "null";
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }
}