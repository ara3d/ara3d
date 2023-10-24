using System;
using System.Diagnostics;

namespace Ara3D.Utils
{
    public class ProcessData
    {
        public ProcessData(Process p)
        {
            ExitCode = p.ExitCode;
            HasExited = p.HasExited;
            Responding = p.Responding;
            ExitTime = p.ExitTime;
            MachineName = p.MachineName;
            WindowTitle = p.MainWindowTitle;
            FileName = p.MainModule?.FileName ?? "";
            Id = p.Id;
            PagedMemorySize = p.PagedMemorySize64;
            NonPagedMemorySize = p.NonpagedSystemMemorySize64;
            VirtualMemorySize = p.VirtualMemorySize64;
            PeakVirtualMemorySize = p.PeakVirtualMemorySize64;
            PeakPagedMemorySize = p.PeakPagedMemorySize64;
            WorkingSet = p.WorkingSet64;
            PeakWorkingSet = p.PeakWorkingSet64;
            PrivateMemorySize = p.PrivateMemorySize64;
            FileVersionInfo = p.MainModule?.FileVersionInfo;
            ModuleName = p.MainModule?.ModuleName ?? "";
        }

        public int ExitCode { get; set; }
        public bool HasExited { get; set; }
        public bool Responding { get; set; }
        public DateTimeOffset ExitTime { get; set; } 
        public string MachineName { get; set; }
        public string FileName { get; set; }
        public string WindowTitle { get; set; }
        public string ModuleName { get; set; }
        public int Id { get; set; }
        public long WorkingSet { get; set; }
        public long PeakWorkingSet { get; set; }
        public long PagedMemorySize { get; set; }
        public long NonPagedMemorySize { get; set; }
        public long PeakPagedMemorySize { get; set; }
        public long VirtualMemorySize { get; set; }
        public long PeakVirtualMemorySize { get; set; }
        public long PrivateMemorySize { get; set; }
        public FileVersionInfo FileVersionInfo { get; set; }
    }
}