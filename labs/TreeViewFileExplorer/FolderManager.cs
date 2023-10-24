using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TreeViewFileExplorer.Enums;

namespace TreeViewFileExplorer
{
    public static class FolderManager
    {
        public static ImageSource GetImageSource(string directory, ItemState folderType) 
            => GetImageSource(directory, new Size(16, 16), folderType);

        public static ImageSource GetImageSource(string directory, Size size, ItemState folderType)
        {
            using var icon = ShellManager.GetIcon(directory, ItemType.Folder, IconSize.Large, folderType);
#pragma warning disable CA1416
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
#pragma warning restore CA1416
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(size.Width, size.Height));
        }
    }
}
