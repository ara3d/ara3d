using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Ara3D.Math;
using Ara3D.Utils;
using Ara3D.Utils.Wpf;
using ColorPicker;

namespace Ara3D.SVG.Creator
{
    /// <summary>
    /// Interaction logic for PropertiesPanel.xaml
    /// </summary>
    public partial class PropertiesPanel : UserControl, INotifyPropertyChanged
    {
        public PropertiesPanel()
        {
            InitializeComponent();
        }

        public object DataObject
        {
            get => GetValue(DataObjectProperty);
            set
            {
                SetValue(DataObjectProperty, value); 
                RecomputeLayout();
            }
        }

        public static DependencyProperty DataObjectProperty = DependencyProperty.Register(nameof(DataObject),
            typeof(object),
            typeof(PropertiesPanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var pp = (PropertiesPanel)sender;
            pp.RecomputeLayout();
        }
        
        public void RecomputeLayout()
        {
            StackPanel.Children.Clear();

            var obj = DataObject;
            if (obj == null) return;
            var t = obj.GetType();
            if (obj is OperatorStack stk)
            {
                {
                    var gen = stk.Generator;
                    var notifier = new PropertyChangeNotifier();
                    var genCtrl = UIGenerator.CreateControlFromProperties("Generator", gen.GetType(), () => gen,
                        _ => OnPropertyChanged(null), notifier);
                    StackPanel.Children.Add(genCtrl);
                }
                foreach (var op in stk.Operators)
                {
                    var notifier = new PropertyChangeNotifier();
                    var ctrl = UIGenerator.CreateControlFromProperties(t.Name.ToUIName(), op.GetType(), () => op,
                        _ => OnPropertyChanged(null), notifier);
                    StackPanel.Children.Add(ctrl);
                }
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
