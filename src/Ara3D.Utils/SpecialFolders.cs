    using System;
    using System.IO;

    namespace Ara3D.Utils
{
    public static class SpecialFolders
    {
        public static DirectoryPath AdminTools => Environment.GetFolderPath(Environment.SpecialFolder.AdminTools);
        public static DirectoryPath ApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static DirectoryPath CDBurning => Environment.GetFolderPath(Environment.SpecialFolder.CDBurning);
        public static DirectoryPath Windows => Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public static DirectoryPath UserProfile => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static DirectoryPath Templates => Environment.GetFolderPath(Environment.SpecialFolder.Templates);
        public static DirectoryPath SystemX86 => Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static DirectoryPath System => Environment.GetFolderPath(Environment.SpecialFolder.System);
        public static DirectoryPath StartMenu => Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
        public static DirectoryPath SendTo => Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
        public static DirectoryPath Resources => Environment.GetFolderPath(Environment.SpecialFolder.Resources);
        public static DirectoryPath Recent => Environment.GetFolderPath(Environment.SpecialFolder.Recent);
        public static DirectoryPath Programs => Environment.GetFolderPath(Environment.SpecialFolder.Programs);
        public static DirectoryPath ProgramFilesX86 => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static DirectoryPath ProgramFiles => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static DirectoryPath PrinterShortcuts => Environment.GetFolderPath(Environment.SpecialFolder.PrinterShortcuts);
        public static DirectoryPath Personal => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static DirectoryPath NetworkShortcuts => Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts);
        public static DirectoryPath MyVideos => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        public static DirectoryPath MyPictures => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static DirectoryPath MyDocuments => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static DirectoryPath LocalizedResources => Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources);
        public static DirectoryPath LocalApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static DirectoryPath InternetCache => Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
        public static DirectoryPath History => Environment.GetFolderPath(Environment.SpecialFolder.History);
        public static DirectoryPath Fonts => Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        public static DirectoryPath Favorites => Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
        public static DirectoryPath DesktopDirectory => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static DirectoryPath Desktop => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static DirectoryPath Cookies => Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
        public static DirectoryPath CommonVideos => Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos);
        public static DirectoryPath CommonTemplates => Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates);
        public static DirectoryPath CommonStartup => Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
        public static DirectoryPath CommonStartMenu => Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
        public static DirectoryPath CommonPrograms => Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
        public static DirectoryPath CommonProgramFilesX86 => Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
        public static DirectoryPath CommonProgramFiles => Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
        public static DirectoryPath CommonPictures => Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
        public static DirectoryPath CommonOemLinks => Environment.GetFolderPath(Environment.SpecialFolder.CommonOemLinks);
        public static DirectoryPath CommonMusic => Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic);
        public static DirectoryPath CommonDocuments => Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        public static DirectoryPath CommonDesktopDirectory => Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        public static DirectoryPath CommonApplicationData => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static DirectoryPath CommonAdminTools => Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools);

        public static DirectoryPath Temp => Path.GetTempPath();
    }
}