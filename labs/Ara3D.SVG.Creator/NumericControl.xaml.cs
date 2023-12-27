using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ara3D.SVG.Creator
{
    public partial class NumericControl : UserControl, INotifyPropertyChanged
    {
        public NumericControl()
        {
            InitializeComponent();
            InnerTextBox.Text = "0";
            DataObject.AddPastingHandler(InnerTextBox, OnPaste);
        }
        
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(NumericControl), new PropertyMetadata(OnValueChanged));

        private Point _capturePoint;
        private double _captureValue;
        public const float Tolerance = 0.0001f;
        public bool DontUpdate = false;

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldVal = (float)(double)e.OldValue;
            var newVal = (float)(double)e.NewValue;
            var diff = System.Math.Abs(oldVal - newVal);
            if (diff < Tolerance)
                return;
            ( d as NumericControl)?.UpdateTextDisplay();
        }

        public double ChangeSize { get; set; } = 5;
        public double PixelToAmount => ChangeSize / 2;

        public void UpdateTextDisplay()
        {
            if (DontUpdate)
                return;

            try
            {
                
                DontUpdate = true;
                InnerTextBox.Text = Value.ToString("0.####");
                OnPropertyChanged(nameof(Value));
            }
            finally
            {
                DontUpdate = false;
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty); 
            set => SetValue(ValueProperty, value); 
        }

        public string Label
        {
            get => LabelButton.Content as string;
            set => LabelButton.Content = value;
        }

        public Brush PolygonBrush
        {
            get => UpPolygon.Stroke;
            set
            {
                UpPolygon.Stroke = value;
                DownPolygon.Stroke = value;
                UpPolygon.Fill = value;
                DownPolygon.Fill = value;
            }
        }

        public GridLength LabelColumnWidth
        {
            get => LabelColumnWidth;
            set => LabelColumnWidth = value;
        }

        public Brush Brush
        {
            get => LabelButton.Background;
            set
            {
                LabelButton.Background = value;
                UpButton.Background = value;
                DownButton.Background = value;
            }
        }
        
        private static bool IsTextAllowed(string text)
            => double.TryParse(text, out _);

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void InnerTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
            => e.Handled = !IsTextAllowed(e.Text);

        private void InnerTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(InnerTextBox.Text, out var value))
                Value = value;
        }

        private void UpButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                StartCapture();
            }
        }

        private void UpButton_OnClick(object sender, RoutedEventArgs e)
        {
            Value += ChangeSize;
        }

        private void DownButton_OnClick(object sender, RoutedEventArgs e)
        {
            Value -= ChangeSize;
        }

        private void StartCapture()
        {
            _capturePoint = Mouse.GetPosition(this);
            _captureValue = Value;
            CaptureMouse();
        }

        private void NumericControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == this)
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    var pt = e.GetPosition(this);
                    var delta = (float)(pt.Y - _capturePoint.Y);
                    if (System.Math.Abs(delta) > 0.001f)
                    {
                        Value = _captureValue - (delta * PixelToAmount);
                    }
                }
                if (e.RightButton == MouseButtonState.Released)
                    ReleaseMouseCapture();
            }
        }

        private void NumericControl_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
