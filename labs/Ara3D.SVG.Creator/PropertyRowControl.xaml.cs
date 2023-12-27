using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using Ara3D.Math;

namespace Ara3D.SVG.Creator
{
    public partial class PropertyRowControl : INotifyPropertyChanged
    {
        public PropertyRowControl()
        {
            InitializeComponent();
        }

        public NumericControl AddProperty(string name, Color backColor, 
            double changeAmount, double defaultValue,  double currentValue, Action<double> onChanged)
        {
            var control = new NumericControl
            {
                Value = currentValue,
                ChangeSize = changeAmount,
                Label = name,
                Brush = new SolidColorBrush(backColor)
            };
            control.PropertyChanged += (sender, _args) =>
            {
                onChanged(control.Value);
                OnPropertyChanged(name);
            };
            Controls.Add(control);
            Grid.Children.Add(control);
            Grid.SetColumn(control, Controls.Count);
            return control;
        }
        
        public string Name
        {
            get => Button.Content as string;
            set => Button.Content = value;
        }

        public List<NumericControl> Controls { get; } = new();
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
