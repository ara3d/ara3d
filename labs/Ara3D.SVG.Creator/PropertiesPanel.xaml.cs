using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ara3D.Utils.Wpf;

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

        public void AddControl(object obj, PropertyInfo pi, UIElementCollection collection)
        {
            collection.AddLabel(pi.Name);
            var val = pi.GetValue(obj);
            if (pi.PropertyType == typeof(string))
            {
                var ctrl = new TextBox();
                ctrl.Text = val as string;
                ctrl.TextChanged += (_, _) =>
                {
                    pi.SetValue(obj, ctrl.Text);
                    OnPropertyChanged(pi.Name);
                };
                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(bool))
            {
                var ctrl = new CheckBox();
                ctrl.IsChecked = val is true;
                ctrl.Checked += (_, _) =>
                {
                    pi.SetValue(obj, ctrl.IsChecked);
                    OnPropertyChanged(pi.Name);
                };
                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(double))
            {
                var ctrl = new Slider();
                ctrl.Minimum = 0;
                ctrl.Maximum = 1000;
                ctrl.Value = (double)val;
                ctrl.ValueChanged += (_, _) =>
                {
                    pi.SetValue(obj, ctrl.Value);
                    OnPropertyChanged(pi.Name);
                };
                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(int))
            {
                var ctrl = new Slider();
                ctrl.Minimum = 0;
                ctrl.Maximum = 100;
                ctrl.LargeChange = 5;
                ctrl.SmallChange = 1;
                ctrl.TickFrequency = 10;
                ctrl.Value = (int)val;
                ctrl.ValueChanged += (_, _) =>
                {
                    pi.SetValue(obj, (int)ctrl.Value);
                    OnPropertyChanged(pi.Name);
                };
                collection.Add(ctrl);
            }
        }

        public void RecomputeLayout()
        {
            var obj = DataObject;
            StackPanel.Children.Clear();

            if (obj == null) return;

            var t = obj.GetType();
            var sp = new StackPanel();

            foreach (var p in t.GetProperties())
            {
                AddControl(obj, p, sp.Children);
            }

            var g = new GroupBox() { Header = t.Name, Content = sp };
            StackPanel.Children.Add(g);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
