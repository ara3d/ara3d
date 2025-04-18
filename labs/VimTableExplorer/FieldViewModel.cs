using System;
using System.Collections.Generic;
using Ara3D.Serialization.VIM;

namespace VimTableExplorer
{
    public class FieldViewModel : ITreeViewModel
    {
        public VimRowData Row;
        public VimColumnData ColumnData;

        public FieldViewModel(VimRowData row, VimColumnData columnData)
        {
            Row = row;
            ColumnData = columnData;
        }

        public string Title => $"{ColumnData.Name} = {ColumnData.GetValue(Row)}";

        public IReadOnlyList<ITreeViewModel> Items
        {
            get
            {
                var table = ColumnData.GetRelatedTable();
                if (table == null) return Array.Empty<ITreeViewModel>();
                var index = (int)ColumnData.GetValue(Row);
                if (index < 0) return Array.Empty<ITreeViewModel>();
                var row = table.GetRow(index);
                var model = new RowViewModel(row);
                var result = new List<ITreeViewModel>(model.Items);
                return result;
            }
        }
    }
}