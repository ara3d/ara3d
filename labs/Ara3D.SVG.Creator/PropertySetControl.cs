using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Ara3D.SVG.Creator;

public class PropertySetControl2 : UserControl, INotifyPropertyChanged
{
    public void AddProperty(string name, Color backColor, Color fontColor)
    {

    }
    
    public NumericControl ControlA;
    public NumericControl ControlB;

    public string PropertyA;
    public string PropertyB;

    public Color ColorA = Color.Brown;
    public Color ColorB = Color.Green;

    public Color FontColor = Color.White;
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
}

