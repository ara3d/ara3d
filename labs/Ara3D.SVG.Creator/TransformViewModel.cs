using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ara3D.Math;

namespace Ara3D.SVG.Creator;

public class TransformViewModel : INotifyPropertyChanged
{
    private double _positionX;
    private double _positionY;
    private double _rotationAngle;
    private double _scaleX = 1.0;
    private double _scaleY = 1.0;
    private double _skewX;
    private double _skewY;

    public Vector2 Position
    {
        get => ((float)_positionX, (float)_positionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }

    public Vector2 Scale
    {
        get => ((float)_scaleX, (float)_scaleY);
        set
        {
            ScaleX = value.X;
            ScaleY = value.Y;
        }
    }

    public Vector2 Skew
    {
        get => ((float)_skewX, (float)_skewY);
        set
        {
            SkewX = value.X;
            SkewY = value.Y;
        }
    }

    public double PositionX
    {
        get => _positionX;
        set => SetField(ref _positionX, value);
    }

    public double PositionY
    {
        get => _positionY;
        set => SetField(ref _positionY, value);
    }

    public double RotationAngle
    {
        get => _rotationAngle;
        set => SetField(ref _rotationAngle, value);
    }

    public double ScaleX
    {
        get => _scaleX;
        set => SetField(ref _scaleX, value);
    }

    public double ScaleY
    {
        get => _scaleY;
        set => SetField(ref _scaleY, value);
    }

    public double SkewX
    {
        get => _skewX;
        set => SetField(ref _skewX, value);
    }

    public double SkewY
    {   
        get => _skewY;
        set => SetField(ref _skewY, value);
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

    public TransformViewModel Clone()
    {
        var x = new TransformViewModel();
        x.ScaleX = ScaleX;
        x.ScaleY = ScaleY;
        x.SkewX = SkewX;
        x.SkewY = SkewY;
        x.RotationAngle = RotationAngle;
        x.PositionX = PositionX;
        x.PositionY = PositionY;
        return x;
    }

    public TransformViewModel LerpTo(TransformViewModel other, float strength)
    {
        PositionX = PositionX.Lerp(other.PositionX, strength);
        PositionY = PositionY.Lerp(other.PositionY, strength);
        RotationAngle = RotationAngle.Lerp(other.RotationAngle, strength);
        ScaleX = ScaleX.Lerp(other.ScaleX, strength);
        ScaleY = ScaleY.Lerp(other.ScaleY, strength);
        SkewX = SkewX.Lerp(other.SkewX, strength);
        SkewY = SkewY.Lerp(other.SkewY, strength);
        return this;
    }
}