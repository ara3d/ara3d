namespace Ara3D.Utils
{
    public readonly struct DirectoryPath
    {
        public string Value { get; }
        public DirectoryPath(string path) => Value = path;
        public override string ToString() => Value;
        public static implicit operator string(DirectoryPath path) => path.Value;
        public static implicit operator DirectoryPath(string path) => new DirectoryPath(path);
    }
}