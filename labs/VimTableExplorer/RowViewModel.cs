using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ara3D.Serialization.VIM;

namespace VimTableExplorer
{
    public class RowViewModel : ITreeViewModel, INotifyPropertyChanged
    {
        public RowViewModel(VimRow row = null)
            => _row = row;

        public string Title 
            => _row?.GetClassName() ?? "No row selected";

        public IReadOnlyList<ITreeViewModel> Items
        {
            get
            {
                if (Row == null)
                    return Array.Empty<ITreeViewModel>();
                var result = Row.Table.Columns.Select(col => ToViewModel(Row, col)).ToList();
                var indexModel = new SimpleViewModel("Index", Row.RowIndex.ToString());
                result.Insert(0, indexModel);
                if (Row.GetClassName() == "Element")
                {
                    var parameters = new ParametersViewModel(Row.Table.Document, Row.RowIndex);
                    result.Add(parameters);

                    var otherTableName = Row.GetString("Type");
                    if (otherTableName == "FamilySymbol")
                    {
                        otherTableName = "FamilyType";
                    }
                    else if (otherTableName.StartsWith("View"))
                    {
                        otherTableName = "View";
                    }

                    var otherTable = Row.Table.Document.FindTable(otherTableName);

                    if (otherTable != null)
                    {
                        if (otherTable.ColumnLookup.ContainsKey("Element"))
                        {
                            var elementColumn = otherTable.GetColumn("Element");
                            for (var i = 0; i < elementColumn.Count; i++)
                            {
                                var tmp = (int)elementColumn[i];
                                if (tmp == Row.RowIndex)
                                {
                                    var row = new VimRow(otherTable, i);
                                    var model = new RowViewModel(row);
                                    result.Add(model);
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }

        public static ITreeViewModel ToViewModel(VimRow row, VimColumn col)
            => new FieldViewModel(row, col);

        private VimRow _row;

        public VimRow Row
        {
            get => _row;
            set
            {
                _row = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}