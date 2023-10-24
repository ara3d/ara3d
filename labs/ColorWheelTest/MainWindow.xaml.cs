using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Color = System.Windows.Media.Color;
using Color = System.Drawing.Color;

namespace ColorWheelTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Recompute();
        }

        public void Recompute()
        {
            if (MyImage == null)
                return;
            var halfX = 300;
            var halfY = 300;
            var bmp = new WriteableBitmap(halfX * 2, halfY * 2, 96, 96, PixelFormats.Bgr32, null);

            var bytes = new byte[bmp.PixelWidth * 4 * bmp.PixelHeight];
            var rowSize = bmp.PixelWidth * 4;
            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    var x = (double)(i - halfX) / halfX;
                    var y = (double)(j - halfY) / halfY;
                    var c = WheelOrSquare.IsChecked == true ? GetColor(x, y) : GetColorSquare(x, y);
                    var n = j * 4 + (i * rowSize);
                    bytes[n] = c.B;
                    bytes[n + 1] = c.G;
                    bytes[n + 2] = c.R;
                }
            }
            var r = new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight);
            bmp.WritePixels(r, bytes, rowSize, 0);
            MyImage.Source = bmp;
        }

        public static (double Hue, double Saturation, double Value) ColorToHSV(Color color)
        {
            var max = Math.Max(color.R, Math.Max(color.G, color.B));
            var min = Math.Min(color.R, Math.Min(color.G, color.B));
            return (color.GetHue(), (max == 0) ? 0 : 1d - (1d * min / max), max / 255d);
        }

        public static Color ColorFromHSV_2(double hue, double saturation, double value)
        {
            var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            var v = Convert.ToInt32(value);
            var p = Convert.ToInt32(value * (1 - saturation));
            var q = Convert.ToInt32(value * (1 - f * saturation));
            var t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            return Color.FromArgb(255, v, p, q);
        }
        
public struct ColorHSL
{
    public readonly double H; // Hue
    public readonly double S; // Saturation
    public readonly double L; // Lightness 

    public ColorHSL(double h, double s, double l)
        => (H, S, L) = (h, s, l);

    public static implicit operator Color(ColorHSL hsl)
        => hsl.ToColorRGB();

    public static byte ToByte(double d)
        => (byte)(d * 0xFF);

    public static Color CreateRGB(double r, double g, double b)
        => Color.FromArgb(0xFF, ToByte(r), ToByte(g), ToByte(b));

    public double V
        => (1.0 - Math.Abs(2.0 * L - 1.0)) * S;

    public double C 
        => V * S;

    public Color ToColorRGB()
    {
        // https://en.wikipedia.org/wiki/HSL_and_HSV
        var h1 = H / 60.0;
        var x = C * (1.0 - Math.Abs((h1 % 2.0) - 1.0));
        var m = L - C/2.0;
        var cm = C + m;
        var xm = x + m;
        if (h1 < 0)
            return CreateRGB(m, m, m);
        if (h1 < 1)
            return CreateRGB(cm, xm, m);
        if (h1 < 2)
            return CreateRGB(xm, cm, m);
        if (h1 < 3)
            return CreateRGB(m, cm, xm);
        if (h1 < 4)
            return CreateRGB(m, xm, cm);
        if (h1 < 5)
            return CreateRGB(xm, m, cm);
        if (h1 <= 6)
            return CreateRGB(cm, m, xm);
        return CreateRGB(m, m, m);
    }
}

        public static byte ToByte(double d)
            => (byte)Math.Clamp((int)(d * 256), 0, 255);

        public static Random Rng = new Random();

        public static void Shuffle<T>(T[] xs)
        {
            for (var i=0; i<xs.Length; i++)
            {
                var index1 = Rng.Next(xs.Length);
                var index2 = Rng.Next(xs.Length);
                if (index1 == index2) continue;
                var x = xs[index2];
                xs[index2] = xs[index1];
                xs[index1] = x;
            }
        }

        public static double[] GenerateRandomDoubles(int count, Func<double, double> f)
        {
            //var r = Enumerable.Range(0, count + 1).Select(i => f((float)i / count)).ToArray();
            //Shuffle(r);
            //return r;
            return Enumerable.Range(0, count + 1).Select(x => Rng.NextDouble()).ToArray();
        }

        public static int NumVals = 1000;

        public static double[] Hues = GenerateRandomDoubles(NumVals, x => x);
        public static double[] Lums = GenerateRandomDoubles(NumVals, x => x * 0.6 + 0.2);
        public static double[] Sats = GenerateRandomDoubles(NumVals, x => x * 0.6 + 0.2);

        public static double Choose(double[] xs, double d)
            => xs[(int)Math.Clamp(d * xs.Length, 0, xs.Length - 1)];


        public double ChooseSat(double d)
            => Choose(Sats, d);

        public double ChooseLum(double d)
            => Slider.Value;
            //=> Choose(Lums, d);

        public double ChooseHue(double d)
            => Choose(Hues, d);

        public static double InverseCubic(double x)
            => (x < 0.5)
                ? 0.5 * EaseOutExpo(x / 2)  
                : 0.5 + 0.5 * EaseInExpo((x / 2 + 0.5));

        public static double EaseInExpo(double x) 
            => x == 0 ? 0 : Math.Pow(2, 10 * x - 10);
        
        public static double EaseOutExpo(double x)
            => x == 1 ? 1 : 1 - Math.Pow(2, -10 * x);

        public static int Rows = 20;
        public static int Cols = 20;

        public static double ToSquare(double x, double y)
            => (x * (double)Cols + (y * (double)Rows) * (double)Cols) / (double)(Rows * Cols);

        public static Color ColorFromHSV(double hue, double saturation, double luminosity)
            => new ColorHSL(hue, saturation, luminosity);

        public Color GetColorSquare(double x, double y)
        {
            x = (x + 1) / 2.0;
            y = (y + 1) / 2.0;
            int row = (int)(x * (double)Rows);
            int col = (int)(y * (double)Cols);
            var index = row * Cols + col;
            var hue = Hues[index] * 360.0;
            var sat = Sats[index]; // CheckBox.IsChecked == true ? 0.8 : Slider.Value);
            var lum = CheckBox.IsChecked == true ? Slider.Value : 0.8;
            return ColorFromHSV(hue, sat, lum);
        }

        public Color GetColor(double x, double y)
        {
            var angle = Math.Atan2(y, x);
            var dist = Math.Sqrt(x * x + y * y);
            if (dist > 1) return Color.White;
            var sat = CheckBox.IsChecked == true ? dist : Slider.Value;
            var lum = CheckBox.IsChecked == true ? Slider.Value : dist;
            var hue = angle * 180.0 / Math.PI;
            hue += 180.0;
            return ColorFromHSV(hue, sat, lum);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Recompute();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Recompute();
        }
    }
}
