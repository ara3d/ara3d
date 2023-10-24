using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Ara3D.Math;
using Ara3D.Serialization.G3D;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;

namespace G3DViewer
{
    public class AttributeStat : ObservableObject
    {
        public string Association { get; set; }
        public int ArrayLength { get; set; }
        public string AttributeType { get; set; }
        public int AttributeTypeIndex { get; set; }
        public int DataArity { get; set; }
        public string DataType { get; set; }
    }

    public class DisplayStats : ObservableObject
    {
        public int NumFaces { get; set; }
        public int NumTriangles { get; set; }
        public int NumDegenerateFaces { get; set; }
        public int NumDegenerateTriangles { get; set; }
        public int NumSmallFaces { get; set; }
        public int NumSmallTriangles { get; set; }
        public int NumVertices { get; set; }
        public int NumMaterialIds { get; set; }
        public int NumObjectIds { get; set; }
        public float LoadTime { get; set; }
        public float VertexBufferGenerationTime { get; set; }
        public int FileSize { get; set; }
        public AABox AABB { get; set; } = new AABox();
        public ObservableCollection<AttributeStat> AttributeStats { get; } = new ObservableCollection<AttributeStat>();

        public const int NumHistogramDivisions = 16;
        public const float SmallTriangleSize = 0.000001f;
        public float MinTriangleArea = float.MaxValue;
        public float MaxTriangleArea = 0.0f;
        public int[] AreaHistogramArray = new int[NumHistogramDivisions];
        public Dictionary<string, float> AreaHistogramLog { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, int> AreaHistogram { get; set; } = new Dictionary<string, int>();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;
        G3D mG3D;

        public static DisplayStats mDisplayStats = new DisplayStats();

        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => {
                if (DataContext is IDisposable)
                {
                    (DataContext as IDisposable).Dispose();
                }
            };
        }

        public void OpenG3D(string FileName)
        {
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            mDisplayStats = new DisplayStats();
            mainViewModel.displayStats = mDisplayStats;
;
            var stopwatch = Stopwatch.StartNew();

            mG3D = G3D.Read(FileName);

            mDisplayStats.LoadTime = stopwatch.ElapsedMilliseconds / 1000.0f;

            stopwatch = Stopwatch.StartNew();

            int index = mainViewModel.AddG3DData(mG3D);

            mDisplayStats.VertexBufferGenerationTime = stopwatch.ElapsedMilliseconds / 1000.0f - mDisplayStats.LoadTime;

            /*
            foreach (var attribute in mG3D.)
            {
                var attributeStat = new AttributeStat();

                attributeStat.Association = ((Association)attribute.Descriptor._association).ToString();
                attributeStat.ArrayLength = attribute.ElementCount();
                attributeStat.AttributeType = ((AttributeType)attribute.Descriptor._attribute_type).ToString();
                attributeStat.AttributeTypeIndex = attribute.Descriptor._attribute_type_index;
                attributeStat.DataArity = attribute.Descriptor._data_arity;
                attributeStat.DataType = ((DataType)attribute.Descriptor._data_type).ToString();

                mDisplayStats.AttributeStats.Add(attributeStat);
            }
            */

            var materialIds = mG3D.Materials;
            if (materialIds != null)
            {
                var materialIdMap = new Dictionary<int, int>();
                for (int materialIdIndex = 0; materialIdIndex < materialIds.Count; materialIdIndex++)
                {
                    int materialId = materialIds[materialIdIndex].Index;
                    if (!materialIdMap.ContainsKey(materialId))
                    {
                        materialIdMap[materialId] = materialId;
                    }
                }
                mDisplayStats.NumMaterialIds = materialIdMap.Count;
            }

            mainViewModel.Title = "";
            mainViewModel.UpdateSubTitle();
            mainViewModel.displayStats = mDisplayStats;
            Chart.ItemsSource = mDisplayStats.AreaHistogramLog;
        }

        public void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "Obj File|*.obj|G3D File|*.g3d|PNG Image|*.png|JPG Image|*.jpg";
                if (dialog.ShowDialog() == true)
                {
                    string extension = Path.GetExtension(dialog.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png")
                    {
                        Direct2DImageFormat format = (extension == ".jpg" ? Direct2DImageFormat.Jpeg : Direct2DImageFormat.Png);
                        ViewportExtensions.SaveScreen(view1, dialog.FileName, format);
                    }
                    else if (extension == ".obj")
                    {
                        mG3D.WriteObj(dialog.FileName);
                    }
                    else if (extension == ".g3d")
                    {
                        mG3D.Write(dialog.FileName);
                    }
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string fileName = files[0];
                OpenFile(fileName);
            }
        }

        public void OpenFile(string FileName)
        {
            if (Path.GetExtension(FileName).ToLower() == ".g3d")
            {
                OpenG3D(FileName);
            }
        }
    }
}
