using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Ara3D.Utils.Roslyn
{
    public class CompilerViewModel : INotifyPropertyChanged
    {
        private CompilerModel _model;
        public CompilerModel Model
        {
            get => _model;
            set { _model = value; TriggerChange(null); }
        }

        public string Directory => Model?.Directory ?? "";
        public bool Compiled => Model?.Compiled ?? false;
        public IEnumerable<string> InputFiles => Model?.InputFiles ?? Enumerable.Repeat("", 0);
        public IEnumerable<string> Diagnostics => Model?.Diagnostics ?? Enumerable.Repeat("", 0);
        public string OutputDll => Model?.OutputDll ?? "";

        private bool _autoCompile;
        public bool AutoCompile
        {
            get => _autoCompile;
            set { _autoCompile = value; TriggerChange(nameof(AutoCompile)); }
        }

        private bool _changed;
        public bool Changed
        {
            get => _changed;
            set { _changed = value; TriggerChange(nameof(Changed)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void TriggerChange(string s)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
    }
}
