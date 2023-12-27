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
            g.Header = name.ToUIName();
            var sp = new StackPanel();
            foreach (var prop in props)
            {
                var ctrl = CreateControlFromProperty(prop, obj, setValue);
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

    public static Control CreateControlFromProperty(PropertyInfo pi, object parentVal, Action<object> setValue)
    {
        var propVal = pi.GetValue(parentVal);
        var ctrl = CreateControl(pi.Name, propVal, pi.PropertyType, (x) =>
        {
            if (pi.CanWrite)
            {
                pi.SetValue(parentVal, x);
                setValue(x);
            }
        });
        return ctrl;
    }

    public static Control CreateControl(string name, object val, Type type, Action<object> setValue)
    {
        if (type == typeof(Size))
        {
            CreateControlFromProperties(name, val, type, setValue);
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
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Scale))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Position))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Angle))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Circle))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Rect))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(RoundedRect))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Ellipse))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Square))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Line))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        else if (type == typeof(Quadratic))
        {
            CreateControlFromProperties(name, val, type, setValue);
        }
        // Primitives 
        else if (type == typeof(string))
        {
            throw new NotImplementedException();
        }
        else if (type == typeof(double))
        {
            CreateRowControl(name, "X", (double)val, x => setValue(x));
        }
        else if (type == typeof(int))
        {
            CreateRowControl(name, "N", (int)val, x => setValue(x));
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
        else
        {

        }

        throw  new NotImplementedException();
    }
}
