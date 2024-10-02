// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageExtension.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Ara3D.Speckle.Wpf
{
    public class ImageExtension : MarkupExtension
    {
        public string Path { get; set; }

        public ImageExtension(string path)
        {
            Path = path;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var source = new BitmapImage(new Uri(Path, UriKind.Relative));
            return new Image() { Source = source, Height=24 };
        }
    }
}