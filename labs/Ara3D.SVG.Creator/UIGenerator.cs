using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ara3D.Math;
using Ara3D.Utils;
using ColorPicker;
using Color = System.Drawing.Color;

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
        double value,
        Action<double> setValue)
    {
        var r = new PropertyRowControl();
        r.Name = name;
        r.AddProperty(fieldName, Colors.LemonChiffon, 5, 0, value, setValue);
        return r;
    }

    public static PropertyRowControl CreateRowControl(
        string name,
        Angle value,
        Action<Angle> setValue)
    {
        var r = new PropertyRowControl();
        r.Name = name;
        r.AddProperty("θ", Colors.LightCyan, 5, 0, value, x => setValue(new Angle(x)));
        return r;
    }

    public static PropertyRowControl CreateRowControl(
        string name,
        string fieldName,
        int value,
        Action<int> setValue)
    {
        var r = new PropertyRowControl();
        r.Name = name;
        r.AddProperty(fieldName, Colors.PaleGreen, 5, 0, value, d => setValue((int)d));
        return r;
    }

    public static Control CreateControlFromProperties(
        string name, object obj, Type type, Action<object> setValue)
    {
        var props = type.GetProperties();
        if (props.Length <= 2 && props.All(p => p.PropertyType.IsNumericType()))
        {
            var r = CreateRowControl(name,
                obj,
                props[0], 
                props.ElementAtOrDefault(1));
            r.PropertyChanged += (sender, args) => setValue(obj);
            return r;
        }
        else
        {
            var g = new GroupBox();
            var notifier = new PropertyChangeNotifier();
            g.Header = name.ToUIName();
            var sp = new StackPanel();

            foreach (var prop in props)
            {
                var ctrl = CreateControlFromProperty(prop, obj, notifier);
                ctrl.Margin = RowMargin;
                sp.Children.Add(ctrl);
            }

            g.Content = sp;
            return g;
        }
    }
    
    public static PropertyRowControl CreateRowControl(
        string name,
        object val, 
        PropertyInfo p1,
        PropertyInfo p2 = null)
    {
        var r = new PropertyRowControl();
        r.Name = name;

        {
            var p1Val = (double)p1.GetValue(val);
            r.AddProperty(p1.Name, Colors.LemonChiffon, 5, 0, p1Val, (x) =>
            {
                if (p1.CanWrite)
                    p1.SetValue(val, x);
            });
        }

        if (p2 != null)
        {
            var p2Val = (double)p2.GetValue(val);
            r.AddProperty(p1.Name, Colors.LemonChiffon, 5, 0, p2Val, (x) =>
            {
                if (p2.CanWrite)
                    p2.SetValue(val, x);
            });
        }

        return r;
    }

    public static Control CreateControlFromProperty(PropertyInfo pi, object parentVal, PropertyChangeNotifier notifier)
    {
        var propVal = pi.GetValue(parentVal);
        var ctrl = CreateControl(pi.Name, propVal, pi.PropertyType, (x) =>
        {
            if (pi.CanWrite)
            {
                var oldVal = pi.GetValue(parentVal);
                if (oldVal.Equals(x))
                    return;
                pi.SetValue(parentVal, x);
                notifier.OnPropertyChanged(pi.Name);
            }
        });
        notifier.PropertyChanged += (s, e) => { }   
        return ctrl;
    }

    public static Control CreateControl(string name, object val, Type type, Action<object> setValue)
    {
        if (type == typeof(Size))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Color))
        {
            var ctrl = new ColorSliders();
            var clr = (Color)val;
            ctrl.SelectedColor = System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
            ctrl.ColorChanged += (_, _) =>
            {
                var clr = ctrl.SelectedColor;
                var drawingColor = Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
                setValue(drawingColor);
            };
        }
        else if (type == typeof(Vector))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Scale))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Position))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Angle))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Circle))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Rect))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(RoundedRect))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Ellipse))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Square))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Line))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Quadratic))
        {
            return CreateControlFromProperties(name, val, type, setValue);
        }
        // Primitives 
        else if (type == typeof(string))
        {
            throw new NotImplementedException();
        }
        else if (type == typeof(double))
        {
            return CreateRowControl(name, "X", (double)val, x => setValue(x));
        }
        else if (type == typeof(int))
        {
            return CreateRowControl(name, "N", (int)val, x => setValue(x));
        }
        else if (type == typeof(bool))
        {
            var ctrl = new CheckBox
            {
                Content = name,
                IsChecked = val is true
            };
            ctrl.Checked += (_, _) => setValue(ctrl.IsChecked);
            ctrl.Unchecked += (_, _) => setValue(ctrl.IsChecked);
            return ctrl;
        }
        else if (type.IsEnum)
        {
            var ctrl = new ComboBox();
            var xs = System.Enum.GetValues(type);
            var names = System.Enum.GetNames(type);
            for (var i=0; i<xs.Length; i++)
            {
                ctrl.Items.Add(names[i]);
            }

            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.combobox?view=windowsdesktop-8.0
            ctrl.IsEditable = true;
            ctrl.IsReadOnly = false;
            ctrl.SelectedIndex = 0;
            ctrl.SelectionChanged += (_, _) => setValue(xs.GetValue(ctrl.SelectedIndex));
            return ctrl;
        }

        throw  new NotImplementedException();
    }
}
