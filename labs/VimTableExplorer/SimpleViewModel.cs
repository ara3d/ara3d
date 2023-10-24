using System;
using System.Collections.Generic;

namespace VimTableExplorer
{
    public class SimpleViewModel : ITreeViewModel
    {
        public SimpleViewModel(string name, string value)
            => (Name, Value) = (name, value);

        public string Name;
        public string Value;

        public string Title => $"{Name} = {Value}";

        public IReadOnlyList<ITreeViewModel> Items => Array.Empty<ITreeViewModel>();
    }
}