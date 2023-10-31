using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Ara3D.Buffers;
using Ara3D.Serialization.VIM;
using Ara3D.Utils;
using TreeViewFileExplorer.ShellClasses;

namespace TreeViewFileExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFileSystemObjects();
            treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        public static BitmapSource LoadPngFromStream(Stream stream)
        {
            var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            return decoder.Frames[0];
        }

        public void LoadPng(byte[] imageData)
        {
            using var mem = new MemoryStream(imageData);
            image.Source = LoadPngFromStream(mem);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            image.Source = null;
            var fso = treeView.SelectedItem as FileSystemObjectInfo;
            if (fso == null)
            {
                label.Content = "No data available";
                return;
            }
            var fsi = fso.FileSystemInfo;
            label.Content = $"File = {fsi.Name}\nLast Accessed = {fsi.LastAccessTime}\nCreated On = {fsi.CreationTime}";

            if (fsi is FileInfo fi)
            {
                label.Content += $"\nSize = {PathUtil.BytesToString(fi.Length)}";
            }

            if (File.Exists(fsi.FullName) && Path.GetExtension(fsi.FullName).ToLowerInvariant() == ".vim")
            {
                try
                {
                    var sd = Serializer.Deserialize(fsi.FullName, new LoadOptions() { SkipGeometry = true });
                    label.Content += $"\nFormat Version = {sd.Header.FileFormatVersion}";
                    label.Content += $"\nSchema Version = {sd.Header.Schema}";
                    label.Content += $"\nGenerator = {sd.Header.Generator}";
                    label.Content += $"\n# Assets = {sd.Assets.Length}";
                    label.Content += $"\n# Entity Tables = {sd.EntityTables.Count}";
                    label.Content += $"\n# Strings {sd.StringTable.Length}";

                    var render = sd.Assets.FirstOrDefault(x => x.Name.ToLowerInvariant().StartsWith("render"));
                    if (render != null)
                    {
                        var bytes = render.ToBytes();
                        //var path = Path.ChangeExtension(Path.GetTempFileName(), "png");
                        //File.WriteAllBytes(path, bytes);
                        LoadPng(bytes);
                    }
                }
                catch (Exception ex)
                {
                    label.Content += $"\nUnable to load VIM: {ex.Message}";
                }
            }
        }

        private void FileSystemObject_AfterExplore(object sender, EventArgs e)
            => Cursor = Cursors.Arrow;

        private void FileSystemObject_BeforeExplore(object sender, EventArgs e)
            => Cursor = Cursors.Wait;

        private void InitializeFileSystemObjects()
        {
            DriveInfo
                .GetDrives()
                .ToList()
                .ForEach(drive =>
                {
                    var fileSystemObject = new FileSystemObjectInfo(drive);
                    fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                    fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                    treeView.Items.Add(fileSystemObject);
                });
            PreSelect(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void PreSelect(string path)
        {
            if (!Directory.Exists(path)) return;
            var driveFileSystemObjectInfo = GetDriveFileSystemObjectInfo(path);
            driveFileSystemObjectInfo.IsExpanded = true;
            PreSelect(driveFileSystemObjectInfo, path);
        }

        private void PreSelect(FileSystemObjectInfo fileSystemObjectInfo,
            string path)
        {
            foreach (var childFileSystemObjectInfo in fileSystemObjectInfo.Children)
            {
                var isParentPath = IsParentPath(path, childFileSystemObjectInfo.FileSystemInfo.FullName);
                if (isParentPath)
                {
                    if (string.Equals(childFileSystemObjectInfo.FileSystemInfo.FullName, path))
                    {
                        /* We found the item for pre-selection */
                    }
                    else
                    {
                        childFileSystemObjectInfo.IsExpanded = true;
                        PreSelect(childFileSystemObjectInfo, path);
                    }
                }
            }
        }

        private FileSystemObjectInfo GetDriveFileSystemObjectInfo(string path)
        {
            var directory = new DirectoryInfo(path);
            var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.RootDirectory.FullName == directory.Root.FullName);
            return GetDriveFileSystemObjectInfo(drive);
        }

        private FileSystemObjectInfo GetDriveFileSystemObjectInfo(DriveInfo drive)
            => treeView.Items.OfType<FileSystemObjectInfo>()
                .FirstOrDefault(fso => fso.FileSystemInfo.FullName == drive.RootDirectory.FullName);

        private bool IsParentPath(string path,  string targetPath)
            => path.StartsWith(targetPath);
    }
}
