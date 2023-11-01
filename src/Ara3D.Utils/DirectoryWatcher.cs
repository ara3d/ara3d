using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Ara3D.Utils
{
    public class DirectoryWatcher
    {
        public FileSystemWatcher Watcher;
        public DirectoryPath Directory => Watcher.Path;

        public IEnumerable<FilePath> GetFiles()
            => Directory.GetFiles(Watcher.Path, Watcher.IncludeSubdirectories);

        private Action OnChange { get; }

        public DirectoryWatcher(string dir, string filter, Action onChange, ISynchronizeInvoke syncObject = null)
            : this(dir, filter, false, onChange, syncObject) { }

        public DirectoryWatcher(string dir, string filter, bool subDirectories, Action onChange, ISynchronizeInvoke syncObject = null)
        {
            Watcher = new FileSystemWatcher(dir)
            {
                NotifyFilter = NotifyFilters.Attributes 
                    | NotifyFilters.CreationTime 
                    | NotifyFilters.DirectoryName 
                    | NotifyFilters.FileName 
                    | NotifyFilters.LastWrite 
                    | NotifyFilters.Size
            };
            Watcher.SynchronizingObject = syncObject;
            Watcher.Filter = filter ?? "*.*";
            Watcher.IncludeSubdirectories = subDirectories;
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Created;
            Watcher.Deleted += Watcher_Deleted;
            Watcher.Renamed += Watcher_Renamed;
            Watcher.Error += Watcher_Error;
            Watcher.EnableRaisingEvents = true;
            OnChange = onChange;
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine($"Error occurred {e}");
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            // TODO: this was triggering a double event.
            //Debug.WriteLine($"Renamed file from {e.OldName} to {e.Name}");
            //OnChange();
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"Deleted file {e.Name}");
            OnChange();
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"Created file {e.Name}");
            OnChange();
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File changed {e.Name}");
            OnChange();
        }
    }
}
