﻿using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreeViewFileExplorer.Enums;

namespace TreeViewFileExplorer
{
    public static class FileManager
    {
        public static ImageSource GetImageSource(string filename) 
            => GetImageSource(filename, new Size(16, 16));

        public static ImageSource GetImageSource(string filename, Size size)
        {
            using var icon = ShellManager.GetIcon(Path.GetExtension(filename), ItemType.File, IconSize.Small, ItemState.Undefined);
#pragma warning disable CA1416
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
#pragma warning restore CA1416
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(size.Width, size.Height));
        }
    }
}
