using System.IO;

namespace Ara3D.Utils
{
    public class ApplicationFolders
    {
        ApplicationData _appData { get; }

        public string AppFolder
            => Path.Combine(_appData.CompanyName, _appData.AppName);

        public ApplicationFolders(ApplicationData appData)
            => _appData = appData;

        public DirectoryPath ApplicationData => SpecialFolders.ApplicationData.RelativeFolder(AppFolder);
        public DirectoryPath CommonApplicationData => SpecialFolders.CommonApplicationData.RelativeFolder(AppFolder);
        public DirectoryPath ProgramFiles => SpecialFolders.ProgramFiles.RelativeFolder(AppFolder);
        public DirectoryPath Documents => SpecialFolders.MyDocuments.RelativeFolder(AppFolder);
        public DirectoryPath Temp => SpecialFolders.Temp.RelativeFolder(AppFolder);
    }
}