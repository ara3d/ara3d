using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ara3D.Math;
using Ara3D.Utils;
using ColorPicker;
using Microsoft.VisualBasic.Devices;
using Color = System.Drawing.Color;
using Mouse = Microsoft.VisualBasic.Devices.Mouse;

namespace Ara3D.SVG.Creator;

public class PropertyChangeNotifier : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class ControlWithUpdater
{
    public ControlWithUpdater(Control control, Action<object> updateAction)
        => (Control, UpdateAction) = (control, updateAction);
    public Control Control { get; }
    public Action<object> UpdateAction { get; }
}

public static class UIGenerator
{
    // Row control.
    // Row control component. 
    // Regular control. 

    public static Thickness RowMargin = new Thickness(0, 1, 0, 1);

   

    public static bool IsNumericType(this Type type)
        => type == typeof(double) 
           || type == typeof(int) 
           || type == typeof(Angle);

    public static PropertyRowControl CreateRowControl(
        string name,
        string fieldName, 
        Func<double> getValue,
        Action<double> setValue,
        PropertyChangeNotifier notifier)
    {
        var r = new PropertyRowControl { Name = name };
        var ctrl = r.AddProperty(fieldName, Colors.LemonChiffon, 5, 0, getValue(), setValue);
        notifier.PropertyChanged += (_, _) => ctrl.Value = getValue();
        return r;
    }

    public static PropertyRowControl CreateRowControl(
        string name,
        Func<Angle> getValue,
        Action<Angle> setValue,
        PropertyChangeNotifier notifier)
    {
        var r = new PropertyRowControl() { Name = name };
        var ctrl = r.AddProperty("θ", Colors.LightCyan, 5, 0, getValue(), x => setValue(new Angle(x)));
        notifier.PropertyChanged += (_, _) => ctrl.Value = getValue();
        return r;
    }

    public static PropertyRowControl CreateRowControl(
        string name,
        Func<StrokeWidth> getValue,
        Action<StrokeWidth> setValue,
        PropertyChangeNotifier notifier)
    {
        var r = new PropertyRowControl() { Name = name };
        var ctrl = r.AddProperty("W", Colors.Aquamarine, 0.2, 0, getValue(), x => setValue(new StrokeWidth(x)));
        notifier.PropertyChanged += (_, _) => ctrl.Value = getValue();
        return r;
    }

    public static PropertyRowControl CreateRowControl(
        string name,
        string fieldName,
        Func<int> getValue,
        Action<int> setValue,
        PropertyChangeNotifier notifier)
    {
        var r = new PropertyRowControl() { Name = name };
        var ctrl = r.AddProperty(fieldName, Colors.PaleGreen, 1, 0, getValue(), d => setValue((int)d));
        notifier.PropertyChanged += (_, _) => ctrl.Value = getValue();
        return r;
    }

    public static Control CreateControlFromProperties(
        string name, Type type, Func<object> getValue, Action<object> setValue, PropertyChangeNotifier notifier)
    {
        var props = type.GetProperties();
        if (props.Length <= 2 && props.All(p => p.PropertyType.IsNumericType()))
        {
            return CreateRowControlFromProperties(name, type, getValue, setValue, props[0], props.ElementAtOrDefault(1), notifier);
        }
        else
        {
            var g = new GroupBox();
            g.Header = name.ToUIName();
            var sp = new StackPanel();

            foreach (var prop in props)
            {
                var ctrl = CreateControlFromProperty(prop, getValue, setValue, notifier);
                ctrl.Margin = RowMargin;
                sp.Children.Add(ctrl);
            }

            notifier.PropertyChanged += (_, _) => setValue(getValue());
            g.Content = sp;
            return g;
        }
    }
    
    public static PropertyRowControl CreateRowControlFromProperties(
        string name,
        Type parentType,
        Func<object> getValue,
        Action<object> setValue,
        PropertyInfo p1,
        PropertyInfo p2,
        PropertyChangeNotifier notifier)
    {
        var r = new PropertyRowControl();
        r.Name = name;

        var defaultValue = 0.0;
        var changeAmount = 5.0;
        if (parentType == typeof(Scale))
        {
            defaultValue = 1.0;
            changeAmount = 0.05;
        }

        {
            if (p1.PropertyType != typeof(double))
                throw new Exception("Only doubles currently supported as properties of numeric row control");

            var p1Val = (double)p1.GetValue(getValue());
            var ctrl = r.AddProperty(p1.Name, Colors.LightPink, changeAmount, defaultValue, p1Val, (x) =>
            {
                if (p1.CanWrite)
                {
                    var val = getValue();
                    p1.SetValue(val, x);
                    setValue(val);
                    notifier.OnPropertyChanged(p1.Name);
                }
            });
            notifier.PropertyChanged += (sender, args) => 
                ctrl.Value = (double)p1.GetValue(getValue());
        }

        if (p2 != null)
        {
            if (p2.PropertyType != typeof(double))
                throw new Exception("Only doubles currently supported as properties of numeric row control");

            var p2Val = (double)p2.GetValue(getValue());
            var ctrl = r.AddProperty(p2.Name, Colors.LightGreen, changeAmount, defaultValue, p2Val, (x) =>
            {
                if (p2.CanWrite)
                { 
                    var val = getValue();
                    p2.SetValue(val, x);
                    setValue(val);
                    notifier.OnPropertyChanged(p2.Name);
                }
            });
            notifier.PropertyChanged += (sender, args) => 
                ctrl.Value = (double)p2.GetValue(getValue());
        }

        return r;
    }

    public static Control CreateControlFromProperty(PropertyInfo pi, Func<object> getParentVal, Action<object> setParentVal, PropertyChangeNotifier notifier)
    {
        var ctrl = CreateControl(pi.Name, pi.PropertyType, () => pi.GetValue(getParentVal()), (x) =>
        {
            if (pi.CanWrite)
            {
                var parentVal = getParentVal();
                var oldVal = pi.GetValue(parentVal);
                if (oldVal.Equals(x))
                    return;
                pi.SetValue(parentVal, x);
                setParentVal(parentVal);
                notifier.OnPropertyChanged(pi.Name);
            }
        }, notifier);
        return ctrl;
    }

    public static void SetColorSliderColor(ColorSliders ctrl, Color clr)
        => ctrl.SelectedColor = System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B);

    public static Control CreateControl(string name, Type type, Func<object> getValue, Action<object> setValue, PropertyChangeNotifier notifier)
    {
        if (type == typeof(Color))
        {
            var ctrl = new ColorSliders();
            SetColorSliderColor(ctrl, (Color)getValue());

            ctrl.ColorChanged += (_, _) =>
            {
                var clr = ctrl.SelectedColor;
                var drawingColor = Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
                setValue(drawingColor);
                notifier.OnPropertyChanged(name);
            };
            notifier.PropertyChanged += (sender, args) => SetColorSliderColor(ctrl, (Color)getValue());
            return ctrl;
        }
        // Primitives 
        else if (type == typeof(string))
        {
            throw new NotImplementedException();
        }
        else if (type == typeof(Angle))
        {
            return CreateRowControl(name, () => (Angle)getValue(), x => setValue(x), notifier);
        }
        else if (type == typeof(StrokeWidth))
        {
            return CreateRowControl(name, () => (StrokeWidth)getValue(), x => setValue(x), notifier);
        }
        else if (type == typeof(double))
        {
            return CreateRowControl(name, "X", () => (double)getValue(), x => setValue(x), notifier);
        }
        else if (type == typeof(float))
        {
            return CreateRowControl(name, "X", () => (float)getValue(), x => setValue((float)x), notifier);
        }
        else if (type == typeof(int))
        {
            return CreateRowControl(name, "N", () => (int)getValue(), x => setValue(x), notifier);
        }
        else if (type == typeof(bool))
        {
            var ctrl = new CheckBox
            {
                Content = name,
                IsChecked = getValue() is true
            };
            ctrl.Checked += (_, _) =>
            {
                setValue(ctrl.IsChecked);
                notifier.OnPropertyChanged(name);
            };
            ctrl.Unchecked += (_, _) =>
            {
                setValue(ctrl.IsChecked);
                notifier.OnPropertyChanged(name);
            };
            notifier.PropertyChanged += (sender, args) => 
                ctrl.IsChecked = getValue() is true;
            return ctrl;
        }
        else if (type.IsEnum)
        {
            var ctrl = new ComboBox();
            var xs = Enum.GetValues(type);
            var names = Enum.GetNames(type);
            for (var i=0; i<xs.Length; i++)
            {
                ctrl.Items.Add(names[i]);
            }

            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.combobox?view=windowsdesktop-8.0
            ctrl.IsEditable = true;
            ctrl.IsReadOnly = false;

            var val = getValue();
            ctrl.SelectedIndex = type.GetEnumIndex(val);
            ctrl.SelectionChanged += (_, _) => setValue(xs.GetValue(ctrl.SelectedIndex));
            notifier.PropertyChanged += (_, _) => type.GetEnumIndex(getValue());
            
            return ctrl;
        }

        return CreateControlFromProperties(name, type, getValue, setValue, notifier);
    }
}
