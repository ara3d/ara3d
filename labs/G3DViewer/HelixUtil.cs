using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace G3DViewer
{
    public static class HelixUtil
    {
        public static Model3DGroup LoadFileModel3DGroup(string filePath)
            => new ModelImporter().Load(filePath);

        public static void RenderToPng(Viewport3D view, string fileName, Brush background = null, int overSamplingMultiplier = 1)
            => view.SaveBitmap(fileName, background, overSamplingMultiplier, BitmapExporter.OutputFormat.Png);

        public static void RenderToJpg(Viewport3D view, string fileName, Brush background = null, int overSamplingMultiplier = 1)
            => view.SaveBitmap(fileName, background, overSamplingMultiplier, BitmapExporter.OutputFormat.Jpg);

        public static void RenderToBmp(Viewport3D view, string fileName, Brush background = null, int overSamplingMultiplier = 1)
            => view.SaveBitmap(fileName, background, overSamplingMultiplier, BitmapExporter.OutputFormat.Bmp);

        public static string[] ValidExportExtensions = { ".obj", ".stl", ".dae", ".xaml", ".xml", ".x3d", ".jpg", ".png" };

        public static string ValidateExportExtension(string filePath)
            => ValidExportExtensions.Contains(Path.GetExtension(filePath ?? "").ToLowerInvariant())
                ? filePath
                : throw new Exception(
                    $"Target export file {filePath} does not have one of the exported extensions {string.Join(", ", ValidExportExtensions)}");

        public static void Export(Viewport3D view, string fileName, Brush background = null)
            => view.Export(ValidateExportExtension(fileName), background);
    }
}