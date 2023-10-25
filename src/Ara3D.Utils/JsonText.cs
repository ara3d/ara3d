namespace Ara3D.Utils
{
    public readonly struct JsonText
    {
        public string Value { get; }
        public JsonText(string path) => Value = path;
        public override string ToString() => Value;
        public static implicit operator string(JsonText text) => text.Value;
        public static implicit operator JsonText(string text) => new JsonText(text);
    }
}