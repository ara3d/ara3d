using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Utils;

namespace Ara3D.SimpleDB
{
    /// <summary>
    /// Stores information about the name of the table, and the types of each
    /// column. This is always created from a type, the type name is the
    /// name of table, and the fields of the type type make up the columns. 
    /// </summary>
    public class TableSchema
    {
        public string Name => Type.Name;
        public readonly int Size;
        public readonly Type Type;
        public readonly List<SchemaEntry> Entries = new List<SchemaEntry>();

        public TableSchema(Type type)
        {
            Type = type;
            foreach (var fi in type.GetFields())
                Entries.Add(new SchemaEntry(fi));
            Size = Entries.Sum(e => e.Size());
        }

        public override string ToString()
            => $"{Name}=({Entries.JoinStringsWithComma()})";

        public int WriteObject(byte[] bytes, ref int offset, object obj, IndexedSet<string> strings)
        {
            var cnt = 0;
            foreach (var e in Entries)
            {
                cnt += e.WriteObject(bytes, ref offset, obj, strings);
            }

            if (cnt != Size)
                throw new Exception($"Expected {Size} but was {cnt}");
            return Size;
        }

        public object ReadObject(byte[] bytes, ref int offset, IReadOnlyList<string> strings)
        {
            var cnt = 0;
            var r = Activator.CreateInstance(Type);
            foreach (var e in Entries)
            {
                cnt += e.ReadObject(bytes, ref offset, r, strings);
            }

            if (cnt != Size)
                throw new Exception($"Expected {Size} but was {cnt}");
            return r;
        }
    }
}