using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ara3D.Buffers;
using Ara3D.Utils;

namespace Ara3D.NarwhalDB
{
    public class Table
    {
        public readonly TableSchema TableSchema;
        public string Name => TableSchema.Name;

        public readonly List<object> Objects
            = new List<object>();

        public Table(TableSchema tableSchema)
            => TableSchema = tableSchema;

        public int Add(object obj)
        {
            Objects.Add(obj);
            return Objects.Count - 1;
        }

        public INamedBuffer ToBuffer(IndexedSet<string> stringTable)
        {
            var data = new byte[Objects.Count * TableSchema.Size];
            var offset = 0;
            foreach (IBinarySerializable obj in Objects)
            {
                obj.Write(data, ref offset, stringTable);
            }

            Debug.Assert(offset == data.Length);
            return data.ToNamedBuffer(TableSchema.Name);
        }

        public static Table Create(IBuffer buffer, Type type, IReadOnlyList<string> strings)
            => Create(buffer, new TableSchema(type), strings);

        public static Table Create(IBuffer buffer, TableSchema schema, IReadOnlyList<string> strings)
            => Create(buffer.ToBytes(), schema, strings);

        public static Table Create(byte[] bytes, TableSchema schema, IReadOnlyList<string> strings)
        {
            var offset = 0;
            var t = new Table(schema);

            while (offset < bytes.Length)
            {
                // TODO: a more efficient way to do this would be to create objects as requested 
                var obj = schema.ReadObject(bytes, ref offset, strings);
                t.Add(obj);
            }

            Debug.Assert(offset == bytes.Length);

            return t;
        }
    }
}