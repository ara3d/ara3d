using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ara3D.Bowerbird.Wpf
{

    public class BowerbirdWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> LogMessages { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Commands { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Files { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}