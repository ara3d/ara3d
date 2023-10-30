// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileDialogService.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Wpf3DViewer
{
    public interface IFileDialogService
    {
        string OpenFileDialog(string initialDirectory, string defaultPath, string filter, string defaultExtension);
        string SaveFileDialog(string initialDirectory, string defaultPath, string filter, string defaultExtension);
    }
}