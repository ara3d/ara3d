﻿using System.Collections.Generic;
using Ara3D.Serialization.VIM;

namespace VimTableExplorer
{
    public class ParametersViewModel : ITreeViewModel
    {
        public ParametersViewModel(VimDocument document, int elementIndex)
        {
            Document = document;
            ElementIndex = elementIndex;
        }

        public VimDocument Document { get; }
        public int ElementIndex { get; }
        public string Title => "Parameters";

        public IReadOnlyList<ITreeViewModel> Items
        {
            get
            {
                var list = new List<SimpleViewModel>();
                if (ElementIndex >= 0)
                {
                    var parameterTable = Document.FindTable("Parameter");
                    var descriptorTable = Document.FindTable("ParameterDescriptor");
                    if (parameterTable != null)
                    {
                        for (var i=0; i < parameterTable.Count; ++i)
                        {
                            var row = (VimRow)parameterTable[i];
                            if ((int)row[2] == ElementIndex)
                            {
                                var valueIndex = (int)row["Value"];
                                var descriptorIndex = (int)row["ParameterDescriptor"];
                                var value = Document.GetString(valueIndex);
                                value = value.Substring(value.IndexOf('|') + 1);
                                var descriptor = descriptorIndex >= 0 
                                    ? (VimRow)descriptorTable[descriptorIndex] 
                                    : null;
                                var nameIndex = descriptor == null ? -1 : (int)descriptor["Name"];
                                var name = Document.GetString(nameIndex);
                                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(value))
                                {
                                    var pvm = new SimpleViewModel(name, value);
                                    list.Add(pvm);
                                }
                            }
                        }
                    }
                }

                return list;
            }
        }
    }
}