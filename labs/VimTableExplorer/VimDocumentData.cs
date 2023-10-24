using System.Collections.Generic;
using System.Linq;
using Ara3D.Serialization.VIM;

namespace VimTableExplorer
{
    public class VimDocumentData
    {
        public SerializableDocument Document { get; }

        public VimDocumentData(SerializableDocument doc)
        {
            Document = doc;
            foreach (var t in Document.EntityTables)
                Add(new VimTableData(this, t));
        }

        public void Add(VimTableData table)
            => _tables.Add(table);

        private List<VimTableData> _tables = new();
        public IReadOnlyList<VimTableData> Tables => _tables;

        public VimTableData FindTable(string name)
            => string.IsNullOrEmpty(name) ? null : Tables.FirstOrDefault(table => table.Name == name);

        public string GetString(int index)
            => index >= 0 ? Document.StringTable[index] : string.Empty;
    }
}