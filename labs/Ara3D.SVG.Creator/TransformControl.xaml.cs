using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualBasic.ApplicationServices;

namespace Ara3D.SVG.Creator
{
    public partial class TransformControl : UserControl, INotifyPropertyChanged
    {
        private TransformViewModel _transform;

        public TransformViewModel Transform
        {
            get => _transform;
            set => SetField(ref _transform, value);
        }

        public TransformControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public static IEnumerable<FrameworkElement> GetDescendants(FrameworkElement fe)
        {
            yield return fe;
            foreach (var x in LogicalTreeHelper.GetChildren(fe).OfType<FrameworkElement>())
            foreach (var x2 in GetDescendants(x))
                yield return x2;
        }
    }
}
