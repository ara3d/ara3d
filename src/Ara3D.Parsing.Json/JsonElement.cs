using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Utils;

namespace Ara3D.Parsing.Json
{
    /// <summary>
    /// A class for representing an element of JsonData.
    /// It provides convenient classes for 
    /// </summary>
    public abstract class JsonElement
    {
        public static JsonElement CreateElement(object x)
        {
            switch (x)
            {
                case null: return Null;
                case bool b: return b ? (JsonElement)True : False;
                case char c: return (JsonString)c.ToString();
                case string s: return (JsonString)s;
                // We truncate decimal values 
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

        public virtual JsonElement this[string name] => Undefined;
        public virtual JsonElement this[int index] => Undefined;
        public virtual int Count => 0;
        public virtual object DotNetValue => null;
        public abstract IEnumerable<JsonElement> GetChildren();
        public static JsonUndefined Undefined => JsonUndefined.Instance;
        public static JsonNull Null => JsonNull.Instance;
        public static JsonFalse False => JsonFalse.Instance;
        public static JsonTrue True => JsonTrue.Instance;
        public static JsonString EmptyString => JsonString.Empty;
        public static JsonNumber Zero => JsonNumber.Zero;
        public static JsonNumber One => JsonNumber.One;
        public bool IsUndefined => this is JsonUndefined;
        public virtual double AsDouble => 0;
        public int AsInteger => (int)AsDouble;
        public float AsFloat => (float)AsDouble;
        public virtual string AsString => ToString();
        public virtual bool AsBool => false;
        public static implicit operator int(JsonElement j) => j.AsInteger;
        public static implicit operator float(JsonElement j) => j.AsFloat;
        public static implicit operator double(JsonElement j) => j.AsDouble;
        public static implicit operator string(JsonElement j) => j.AsString;
        public static implicit operator bool(JsonElement j) => j.AsBool;
    }

    public class JsonField : JsonElement
    {
        public JsonField(JsonString key, JsonElement value) => (Key, Value) = (key, value); 
        public JsonString Key { get; }
        public override object DotNetValue => this.Value.DotNetValue;
        public JsonElement Value { get; }
        public override string ToString() => $"{Key}:{Value}";
        public static JsonField Create(JsonString key, JsonElement value) => (key, value);
        public static JsonField Create(KeyValuePair<string, object> keyValue) => Create(keyValue.Key, CreateElement(keyValue.Value));
        public static implicit operator (JsonString, JsonElement)(JsonField field) => (field.Key, field.Value);
        public static implicit operator JsonField((JsonString, JsonElement) tuple) => new JsonField(tuple.Item1, tuple.Item2);
        public override IEnumerable<JsonElement> GetChildren() => new[] { Key, Value };
        public override double AsDouble => Value.AsDouble;
        public override bool AsBool => Value.AsBool;
    }

    public class JsonObject : JsonElement
    {
        public JsonObject(IReadOnlyList<JsonField> fields) => Fields = fields;
        public IReadOnlyList<JsonField> Fields { get; }
        public override string ToString() => "{" + Fields.JoinStringsWithComma() + "}";
        public override int Count => Fields.Count;
        public override object DotNetValue => Fields;
        public override JsonElement this[int index] => Fields[index].Value;
        public override JsonElement this[string name] => Fields.FirstOrDefault(f => f.Key.Value == name)?.Value ?? Undefined;
        public static JsonObject Create(params JsonField[] fields) => new JsonObject(fields);
        public static JsonObject Create(IEnumerable<JsonField> fields) => Create(fields.ToArray());
        public static JsonObject Create(IEnumerable<KeyValuePair<string, object>> keysAndValues) => Create(keysAndValues.Select(JsonField.Create));
        public static JsonObject Create(object o) => Create(o.PublicFieldsAndFieldBackedPropertiesToDictionary());
        public override IEnumerable<JsonElement> GetChildren() => Fields;
        public override double AsDouble => Fields.Count;
        public override bool AsBool => Fields.Count > 0;
    }

    public class JsonArray : JsonElement
    {
        public JsonArray(IReadOnlyList<JsonElement> elements) => Elements = elements;
        public IReadOnlyList<JsonElement> Elements { get; }
        public override object DotNetValue => Elements;
        public override string ToString() => "[" + Elements.JoinStringsWithComma() + "]";
        public override int Count => Elements.Count;
        public override JsonElement this[int index] => Elements[index];
        public static JsonArray Create(IEnumerable<JsonElement> elements) => new JsonArray(elements.ToList());
        public static JsonArray Create(params JsonElement[] elements) => new JsonArray(elements);
        public static JsonArray Create(IEnumerator enumerator) => Create(enumerator.ToList(CreateElement));
        public static implicit operator JsonElement[](JsonArray array) => array.Elements.ToArray();
        public static implicit operator JsonArray(JsonElement[] array) => new JsonArray(array);
        public override IEnumerable<JsonElement> GetChildren() => Elements;
        public override double AsDouble => Elements.Count;
        public override bool AsBool => Elements.Count > 0;
    }

    public class JsonString : JsonElement
    {
        public JsonString(string value) => Value = value.EscapeQuotes();
        public string Value { get; }
        public override object DotNetValue => Value;
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
        public override double AsDouble => double.TryParse(Value, out var r) ? r : 0;
        public override string AsString => Value.UnescapeQuotes();
        public override bool AsBool => !Value.IsNullOrEmpty();
    }

    public class JsonNumber : JsonElement
    {
        public JsonNumber(double value) => Value = value;
        public double Value { get; }
        public override object DotNetValue => Value;
        public static readonly JsonNumber Zero = new JsonNumber(0);
        public static readonly JsonNumber One = new JsonNumber(1);
        public override string ToString() => Value.ToString();
        public static JsonNumber Create(string value) => double.Parse(value);
        public static implicit operator double(JsonNumber value) => value.Value;
        public static implicit operator JsonNumber(double value) => new JsonNumber(value);
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
        public override double AsDouble => Value;
        public override bool AsBool => Value != 0;
    }

    public class JsonTrue : JsonElement
    {
        public static readonly JsonTrue Instance = new JsonTrue();
        public override object DotNetValue => true;
        public override string ToString() => "true";
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
        public override double AsDouble => 1;
        public override bool AsBool => true;
    }

    public class JsonFalse : JsonElement
    {
        public static readonly JsonFalse Instance = new JsonFalse();
        public override object DotNetValue => false;
        public override string ToString() => "false";
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
        public override double AsDouble => 0;
        public override bool AsBool => false;
    }

    public class JsonNull : JsonElement
    {
        public static readonly JsonNull Instance = new JsonNull();
        public override string ToString() => "null";
        public override object DotNetValue => null;
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }

    public class JsonUndefined : JsonElement
    {
        public static readonly JsonUndefined Instance = new JsonUndefined();
        public override string ToString() => "undefined";
        public override object DotNetValue => this;
        public override IEnumerable<JsonElement> GetChildren() => Array.Empty<JsonElement>();
    }
}