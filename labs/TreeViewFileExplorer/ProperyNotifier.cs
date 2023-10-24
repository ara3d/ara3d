using System;
using System.ComponentModel;

namespace TreeViewFileExplorer
{
    [Serializable]
    public abstract class PropertyNotifier : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
