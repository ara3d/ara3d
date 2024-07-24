using System;
using System.Collections.Generic;
using System.Reflection;
using Ara3D.Utils;

namespace Ara3D.SimpleDB
{
    public class SchemaEntry
    {
        public readonly string Name;
        public readonly FieldType Type;
        public readonly FieldInfo Field;

        public SchemaEntry(FieldInfo fi)
        {
            Field = fi;
            Name = fi.Name;
            Type = GetSchemaType(fi.FieldType);
        }

        public static FieldType GetSchemaType(Type type)
        {
            if (type.Equals(typeof(int)))
                return FieldType.Int32;

            if (type.Equals(typeof(string)))
                return FieldType.String;

            throw new Exception($"Not supported type for schema: {type}");
        }

        public override string ToString()
            => $"{Name}:{Type}";

        public int Size()
            => Type == FieldType.Guid ? 16 : 4;

        public int WriteObject(byte[] bytes, ref int offset, object obj, IndexedSet<string> strings)
        {
            if (Type == FieldType.Int32)
            {
                var intVal = (int)Field.GetValue(obj);
                return SimpleDatabase.WriteInt(bytes, ref offset, intVal);
            }

            if (Type == FieldType.String)
            {
                var strVal = (string)Field.GetValue(obj);
                var index = strings.Add(strVal ?? "");
                return SimpleDatabase.WriteInt(bytes, ref offset, index);
            }

            throw new Exception($"Not a supported type {Type}");
        }

        public int ReadObject(byte[] bytes, ref int offset, object obj, IReadOnlyList<string> strings)
        {
            if (Type == FieldType.Int32)
            {
                var intVal = SimpleDatabase.ReadInt(bytes, ref offset);
                Field.SetValue(obj, intVal);
                return Size();
            }

            if (Type == FieldType.String)
            {
                var intVal = SimpleDatabase.ReadInt(bytes, ref offset);
                var strVal = strings[intVal];
                Field.SetValue(obj, strVal);
                return Size();
            }

            throw new Exception($"Not a supported type {Type}");
        }
    }
}