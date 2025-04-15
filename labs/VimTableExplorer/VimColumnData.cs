using System;
using System.ComponentModel;
using Ara3D.Buffers;
using Ara3D.Serialization.VIM;

namespace VimTableExplorer
{
    public class VimColumnData : PropertyDescriptor
    {
        public VimTableData Table { get; }
        public INamedBuffer Buffer { get; }
        public int ColumnIndex { get; }
        public Type ColumnType { get; }
        public string RelatedTableName { get; }

        public VimColumnData(VimTableData table, INamedBuffer buffer, int index)
            : base(buffer.Name.GetColumnNameFromBufferName(), null)
        {
            Table = table;
            Buffer = buffer;
            ColumnIndex = index;

            ColumnType = buffer.GetTypePrefix() switch
            {
                VimConstants.IntColumnNameTypePrefix => typeof(int),
                VimConstants.ByteColumnNameTypePrefix => typeof(byte),
                VimConstants.FloatColumnNameTypePrefix => typeof(float),
                VimConstants.DoubleColumnNameTypePrefix => typeof(double),
                VimConstants.StringColumnNameTypePrefix => typeof(string),
                VimConstants.IndexColumnNameTypePrefix => typeof(int),             
                VimConstants.LongColumnNameTypePrefix => typeof(long),
                _ => throw new ArgumentException($"Unrecognized VIM column type {buffer.Name}")
            };

            RelatedTableName = buffer.Name.GetRelatedTableName();
        }

        public VimTableData GetRelatedTable()
            => Table.Document.FindTable(RelatedTableName);

        public override bool CanResetValue(object component) 
            => false;

        public override object GetValue(object component)
        {
            if (component is VimRowData vtr)
            {
                if (vtr.RowIndex < 0 || vtr.RowIndex >= Count)
                    throw new Exception("Row index out of range");
                if (ColumnType == typeof(string))
                {
                    var span = Buffer.Span<int>();
                    var stringIndex = span[vtr.RowIndex];
                    return Table.GetString(stringIndex);
                }

                return Buffer[vtr.RowIndex];
            }
            throw new ArgumentException("Incorrect component type", nameof(component));
        }

        public override void ResetValue(object component)
            => throw new NotImplementedException();

        public override void SetValue(object component, object value)
            => throw new NotImplementedException();

        public override bool ShouldSerializeValue(object component)
            => false;

        public override Type ComponentType 
            => typeof(VimRowData);

        public override bool IsReadOnly 
            => true;

        public override Type PropertyType
            => ColumnType;

        public int Count 
            => Buffer.Count;

        public object this[int n]
            => Buffer[n];
    }
}