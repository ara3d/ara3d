using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ara3D.Math;
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

        public void AddControl(object obj, PropertyInfo pi, UIElementCollection collection)
        {
            var val = pi.GetValue(obj);
            if (pi.PropertyType == typeof(string))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new TextBox();
                ctrl.Text = val as string;
                if (pi.CanWrite)
                {
                    ctrl.TextChanged += (_, _) =>
                    {
                        pi.SetValue(obj, ctrl.Text);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(bool))
            {
                var ctrl = new CheckBox() { Content = pi.Name };
                ctrl.IsChecked = val is true;
                if (pi.CanWrite)
                {
                    ctrl.Checked += (_, _) =>
                    {
                        pi.SetValue(obj, ctrl.IsChecked);
                        OnPropertyChanged(pi.Name);
                    };
                    ctrl.Unchecked += (_, _) =>
                    {
                        pi.SetValue(obj, ctrl.IsChecked);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(double))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new Slider();
                ctrl.Minimum = 0;
                ctrl.Maximum = 100;
                ctrl.Value = (double)val;
                if (pi.CanWrite)
                {
                    ctrl.ValueChanged += (_, _) =>
                    {
                        pi.SetValue(obj, ctrl.Value);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(float))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new Slider();
                ctrl.Minimum = 0;
                ctrl.Maximum = 100;
                ctrl.Value = (float)val;
                if (pi.CanWrite)
                {
                    ctrl.ValueChanged += (_, _) =>
                    {
                        pi.SetValue(obj, (float)ctrl.Value);
                        OnPropertyChanged(pi.Name);
                    };
                }
                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(int))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new Slider();
                ctrl.Minimum = 0;
                ctrl.Maximum = 100;
                ctrl.LargeChange = 5;
                ctrl.SmallChange = 1;
                ctrl.TickFrequency = 10;
                ctrl.Value = (int)val;

                if (pi.CanWrite)
                {
                    ctrl.ValueChanged += (_, _) =>
                    {
                        pi.SetValue(obj, (int)ctrl.Value);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(System.Drawing.Color))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new ColorSliders();
                var clr = (System.Drawing.Color)val;
                ctrl.SelectedColor = Color.FromArgb(clr.A, clr.R, clr.G, clr.B);

                if (pi.CanWrite)
                {
                    ctrl.ColorChanged += (_, _) =>
                    {
                        var clr = ctrl.SelectedColor;
                        var drawingColor = System.Drawing.Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
                        pi.SetValue(obj, drawingColor);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(Vector2))
            {
                collection.AddLabel(pi.Name);
                var ctrl = new Vector2Control();
                ctrl.Value = (Vector2)val;
                if (pi.CanWrite)
                {
                    ctrl.PropertyChanged += (_, _) =>
                    {
                        pi.SetValue(obj, ctrl.Value);
                        OnPropertyChanged(pi.Name);
                    };
                }

                collection.Add(ctrl);
            }
            else if (pi.PropertyType == typeof(FunctionRendererParameters)
                    || typeof(Operator).IsAssignableFrom(pi.PropertyType)
                    || typeof(Generator).IsAssignableFrom(pi.PropertyType)
                    )
            {
                if (val == null) return;
                var ctrl = ToGroupBox(val, pi.Name);
                if (ctrl != null)
                    collection.Add(ctrl);
            }
            else if (val is List<Operator> mods)
            {
                foreach (var mod in mods)
                {
                    var ctrl = ToGroupBox(mod, mod.GetType().Name);
                    if (ctrl != null)
                        collection.Add(ctrl);
                }
            }
            else
            {
                collection.Add(new Label() { Content = $"<{pi.PropertyType}>" });

            }
        }

        public GroupBox ToGroupBox(object obj, string name)
        {
            if (obj == null) 
                return null;

            var t = obj.GetType();
            var sp = new StackPanel();

            foreach (var p in t.GetProperties())
            {
                AddControl(obj, p, sp.Children);
            }

            return new GroupBox() { Header = name, Content = sp };
        }

        public void RecomputeLayout()
        {
            StackPanel.Children.Clear();

            var obj = DataObject;
            StackPanel.Children.Add(ToGroupBox(obj, null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
