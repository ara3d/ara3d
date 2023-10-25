using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Ara3D.Utils
{
    /// <summary>
    /// Todo: we need to replace strings with FilePath and DirectoryPath
    /// </summary>
    public static class FileUtil
    {
        public static FileVersionInfo ToFileVersion(this string filePath)
            => FileVersionInfo.GetVersionInfo(filePath);

        /// <summary>
        /// Generates a Regular Expression character set from an array of characters
        /// </summary>
        public static Regex CharSetToRegex(params char[] chars)
            => new Regex($"[{Regex.Escape(new string(chars))}]");

        /// <summary>
        /// Creates a regular expression for finding illegal file name characters.
        /// </summary>
        public static Regex InvalidFileNameRegex =>
            CharSetToRegex(Path.GetInvalidFileNameChars());

        /// <summary>
        /// The normalized DateTime format, suitable for inclusion in a filename.
        /// </summary>
        public const string NormalizedDateTimeFormat = "yyyy-MM-dd_HH-mm-ss";

        /// <summary>
        /// Returns the normalized representation of the given DateTime.
        /// </summary>
        public static string ToNormalizedString(this DateTime dateTime)
            => dateTime.ToString(NormalizedDateTimeFormat);

        /// <summary>
        /// Returns the current date-time in a format appropriate for appending to files.
        /// </summary>
        public static string GetTimeStamp()
            => DateTime.Now.ToNormalizedString();

        public static string GetFileName(this FilePath filePath)
            => Path.GetFileName(filePath);

        public static string GetFileNameWithoutExtension(this FilePath filePath)
            => Path.GetFileNameWithoutExtension(filePath);

        public static string GetExtension(this FilePath filePath)
            => Path.GetExtension(filePath);

        public static bool HasExtension(this FilePath filePath, string extension)
            => filePath.Value.ToLowerInvariant().EndsWith(extension.ToLowerInvariant());

        public static string GetExtensionLower(this FilePath filePath)
            => Path.GetExtension(filePath).ToLowerInvariant();

        public static FilePath ChangeBaseName(this FilePath filePath, Func<string, string> f)
            => filePath.GetDirectory().RelativeFile(
                f(filePath.GetFileNameWithoutExtension()) + f(filePath.GetExtension()));

        /// <summary>
        /// Appends a time stamp to the time stamped filename and extension.
        /// </summary>
        public static string TimeStampedFileName(this FilePath filePath)
            => ChangeBaseName(filePath, name => $"{name}-{GetTimeStamp()}");

        public static FilePath Move(this FilePath filePath, FilePath destination)
        {
            File.Move(filePath, destination);
            return destination;
        }

        public static string CopyToFolder(this string path, string dir, bool dontCreate = false)
        {
            if (!dontCreate)
                Directory.CreateDirectory(dir);
            var newPath = Path.Combine(dir, Path.GetFileName(path));
            File.Copy(path, newPath);
            return newPath;
        }

        public static string MoveToFolder(this string path, string dir, bool dontCreate = false)
        {
            if (!dontCreate)
                Directory.CreateDirectory(dir);
            var newPath = Path.Combine(dir, Path.GetFileName(path));
            File.Move(path, newPath);
            return newPath;
        }

        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (var fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Returns the size of the directory in bytes.
        /// </summary>
        public static long GetDirectorySizeInBytes(string directoryPath)
            => GetAllFiles(directoryPath).Aggregate(0L, (acc, path) =>
            {
                var fileInfo = new FileInfo(path);
                return fileInfo.Exists ? acc + fileInfo.Length : acc;
            });

        /// <summary>
        /// Given a file name, returns a new file name that has the parent directory name prepended to it.
        /// </summary>
        public static string GetFileNameWithParentDirectory(this string file, string sep = "-")
        {
            var baseName = Path.GetFileName(file);
            var dir = Path.GetDirectoryName(file);
            var dirName = new DirectoryInfo(dir).Name;
            return $"{dirName}{sep}{baseName}";
        }

        // Improved answer over:
        // https://stackoverflow.com/questions/211008/c-sharp-file-management
        // https://stackoverflow.com/questions/1358510/how-to-compare-2-files-fast-using-net?noredirect=1&lq=1
        // Should be faster than the SHA version, with no chance of a mismatch.
        public static bool CompareFiles(string filePath1, string filePath2)
        {
            if (!File.Exists(filePath2) || !File.Exists(filePath2))
                return false;

            if (new FileInfo(filePath1).Length != new FileInfo(filePath2).Length)
                return false;

            // Default buffer size of File stream * 16.
            // Profiling revealed this to be faster than the default
            const int bufferSize = 4096 * 16;
            var buf1 = new byte[bufferSize];
            var buf2 = new byte[bufferSize];

            // open both for reading
            using (var stream1 = OpenFileStreamReading(filePath1, bufferSize))
            using (var stream2 = OpenFileStreamReading(filePath1, bufferSize))
            {
                // Read buffers (need to be careful because of contract of stream.Read)
                var tmp1 = stream1.SafeRead(buf1, 0, bufferSize);
                var tmp2 = stream2.SafeRead(buf2, 0, bufferSize);

                // Check that we read the same size
                if (tmp1 != tmp2) return false;

                // Compare the bytes
                for (var i = 0; i < tmp1; ++i)
                {
                    if (buf1[i] != buf2[i])
                        return false;
                }
            }

            return true;
        }

        public static byte[] FileSHA256(string filePath)
            => SHA256.Create().ComputeHash(File.OpenRead(filePath));

        /// <summary>
        /// Returns all the files in the given directory and optionally its subdirectories,
        /// or just returns the passed file.
        /// </summary>
        public static IEnumerable<string> GetFiles(string path, string searchPattern = "*", bool recurse = false)
            => File.Exists(path)
                ? Enumerable.Repeat(path, 1)
                : Directory.Exists(path)
                    ? Directory.EnumerateFiles(path, searchPattern,
                        recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    : Array.Empty<string>();

        /// <summary>
        /// Deletes the file or directory (recursively) at the given path.
        /// </summary>
        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        /// <summary>
        /// Deletes all contents in a folder
        /// https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
        /// </summary>
        public static void DeleteFolderContents(string folderPath)
        {
            var di = new DirectoryInfo(folderPath);
            foreach (var dir in di.EnumerateDirectories().AsParallel())
                DeleteFolderAndAllContents(dir.FullName);
            foreach (var file in di.EnumerateFiles().AsParallel())
                file.Delete();
        }

        /// <summary>
        /// Deletes everything in a folder and then the folder.
        /// </summary>
        public static void DeleteFolderAndAllContents(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;
            DeleteFolderContents(folderPath);
            Directory.Delete(folderPath);
        }

        /// <summary>
        /// Creates a directory if needed, or clears all of its contents otherwise
        /// </summary>
        public static string CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return dirPath;
        }

        /// <summary>
        /// Create the directory for the given filepath if it doesn't exist.
        /// </summary>
        public static string CreateFileDirectory(string filepath)
        {
            var dirPath = Path.GetDirectoryName(filepath);
            if (dirPath != null && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return filepath;
        }

        /// <summary>
        /// Creates a directory if needed, or clears all of its contents otherwise
        /// </summary>
        public static string CreateAndClearDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            else
                DeleteFolderContents(dirPath);
            return dirPath;
        }

        /// <summary>
        /// Returns true if the given directory contains no files or if the directory does not exist.
        /// </summary>
        public static bool DirectoryIsEmpty(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return true;
            }

            return Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories).Length == 0;
        }

        /// <summary>
        /// Deletes the target filepath if it exists and creates the containing directory.
        /// </summary>
        public static string DeleteFilepathAndCreateParentDirectory(string filepath)
        {
            // Delete the filepath (or directory) if it already exists.
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            else if (Directory.Exists(filepath))
            {
                Directory.Delete(filepath, true);
            }

            // Create the target directory containing the output path.
            var fullPath = Path.GetFullPath(filepath);
            var fullDirPath = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(fullDirPath);

            return filepath;
        }

        /// <summary>
        /// Returns the files in the given directory matching the given predicate function.
        /// </summary>
        public static IEnumerable<FileInfo> GetFilesInDirectoryWhere(string dirPath, Func<FileInfo, bool> predicateFn,
            bool recurse = true)
            => !Directory.Exists(dirPath)
                ? Enumerable.Empty<FileInfo>()
                : Directory.GetFiles(dirPath, "*",
                        recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .Select(f => new FileInfo(f))
                    .Where(predicateFn);

        /// <summary>
        /// Useful quick test to assure that we can create a file in the folder and write to it.
        /// </summary>
        public static void TestWrite(string folder)
        {
            var fileName = Path.Combine(folder, "_deleteme_.tmp");
            File.WriteAllText(fileName, "test");
            File.Delete(fileName);
        }

        /// Convert a string to a valid name
        /// https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
        /// https://stackoverflow.com/questions/2230826/remove-invalid-disallowed-bad-characters-from-filename-or-directory-folder?noredirect=1&lq=1
        /// https://stackoverflow.com/questions/10898338/c-sharp-string-replace-to-remove-illegal-characters?noredirect=1&lq=1
        /// </summary>
        public static string ToValidFileName(this string s)
            => InvalidFileNameRegex.Replace(s, m => "_");

        /// <summary>
        /// Returns true if the string has any invalid file name chars
        /// </summary>
        public static bool HasInvalidFileNameChas(this FilePath filePath)
            => InvalidFileNameRegex.Match(filePath).Success;

        /// <summary>
        /// Returns the name of the outer most folder given a file path or a directory path
        /// https://stackoverflow.com/questions/3736462/getting-the-folder-name-from-a-path
        /// </summary>
        public static string DirectoryName(string filePath)
            => new DirectoryInfo(filePath).Name;

        /// <summary>
        /// Changes the directory and the extension of a file
        /// </summary>
        public static string ChangeDirectoryAndExt(string filePath, string newFolder, string newExt)
            => Path.ChangeExtension(ChangeDirectory(filePath, newFolder), newExt);

        /// <summary>
        /// Changes the directory of a file
        /// </summary>
        public static string ChangeDirectory(string filePath, string newFolder)
            => Path.Combine(newFolder, Path.GetFileName(filePath));

        public static FilePath ChangeExtension(this FilePath filePath, string ext)
            => Path.ChangeExtension(filePath, ext);

        public static FilePath StripExtension(this FilePath filePath)
            => Path.ChangeExtension(filePath, null);

        public static DirectoryPath GetDirectory(this FilePath filePath)
            => DirectoryName(filePath);

        /// <summary>
        /// Finds an a parent (or ancestor) directory that satisfies the criteria
        /// </summary>
        public static DirectoryInfo FindParentDirectory(DirectoryInfo currentDirectory,
            Func<DirectoryInfo, bool> predicate, int maxIterations = 15)
        {
            for (var i = 0; i < maxIterations; ++i)
            {
                if (currentDirectory is null)
                    return null;

                // base case: predicate matches.
                if (predicate(currentDirectory))
                    return currentDirectory;

                // recursive case: go up one directory.
                currentDirectory = currentDirectory?.Parent;
            }

            return null;
        }

        /// <summary>
        /// Advances a stream a fixed number of bytes.
        /// </summary>
        public static void Advance(this Stream stream, long count, int bufferSize = 4096)
        {
            if (stream.CanSeek)
            {
                stream.Position += count;
                return;
            }

            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, count))) > 0)
            {
                count -= bytesRead;
            }
        }

        /// <summary>
        /// Useful quick test to assure that we can create a file in the folder and write to it.
        /// </summary>
        public static void TestWrite(this DirectoryInfo di)
            => TestWrite(di.FullName);

        public static bool HasWildCard(string filePath)
            => filePath.Contains("*") || filePath.Contains("?");

        /// <summary>
        /// Returns all the files in the given directory and its subdirectories,
        /// or just returns the passed file.
        /// </summary>
        public static IEnumerable<string> GetAllFiles(string path, string searchPattern = "*")
            => GetFiles(path, searchPattern, true);

        // File size reporting

        static readonly string[] ByteSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB

        /// Improved version of https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
        public static string BytesToString(long byteCount, int numPlacesToRound = 1)
        {
            if (byteCount == 0) return "0B";
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), numPlacesToRound);
            return $"{(Math.Sign(byteCount) * num).ToString($"F{numPlacesToRound}")}{ByteSuffixes[place]}";
        }

        /// <summary>
        /// Returns the file size in bytes, or 0 if there is no file.
        /// </summary>
        public static long FileSize(string fileName)
            => File.Exists(fileName) ? new FileInfo(fileName).Length : 0;

        /// <summary>
        /// Returns the file size in bytes, or 0 if there is no file.
        /// </summary>
        public static string FileSizeAsString(string fileName, int numPlacesToShow = 1)
            => BytesToString(FileSize(fileName), numPlacesToShow);

        /// <summary>
        /// Returns the file size in bytes, or 0 if there is no file.
        /// </summary>
        public static string FileSizeAsString(this FileInfo f, int numPlacesToShow = 1)
            => BytesToString(f.Length, numPlacesToShow);

        /// <summary>
        /// Returns the total file size of all files given
        /// </summary>
        public static long TotalFileSize(IEnumerable<string> files)
            => files.Sum(FileSize);

        /// <summary>
        /// Returns the total file size of all files given as a human readable string
        /// </summary>
        public static string TotalFileSizeAsString(IEnumerable<string> files, int numPlacesToShow = 1)
            => BytesToString(TotalFileSize(files), numPlacesToShow);

        /// <summary>
        /// Returns the most recently written to sub-folder
        /// </summary>
        public static string GetMostRecentSubFolder(string folderPath)
            => Directory.GetDirectories(folderPath).OrderByDescending(f => new DirectoryInfo(f).LastWriteTime)
                .FirstOrDefault();


        /// <summary>
        /// Applies a function to transform a function name (withtout extension) leaving it in the same folder and keeping the original extension
        /// </summary>
        public static string TransformFileName(string filePath, Func<string, string> func)
            => Path.Combine(Path.GetDirectoryName(filePath) ?? "",
                func(Path.GetFileNameWithoutExtension(filePath)) + Path.GetExtension(filePath));

        /// <summary>
        /// Prepends text to the file name keeping it in the same folder and with the same extension
        /// </summary>
        public static string PrependFileName(string filePath, string text)
            => TransformFileName(filePath, name => text + name);

        /// <summary>
        /// Prepends text to the file name keeping it in the same folder and with the same extension
        /// </summary>
        public static string AppendFileName(string filePath, string text)
            => TransformFileName(filePath, name => name + text);

        /// <summary>
        /// Returns all the lines of all the files
        /// </summary>
        public static IEnumerable<string> ReadManyLines(IEnumerable<string> fileNames)
            => fileNames.SelectMany(File.ReadLines);

        /// <summary>
        /// Concatenates the contents of all the files and writes them to a new file.
        /// </summary>
        public static void ConcatFiles(string filePath, IEnumerable<string> fileNames)
            => File.WriteAllLines(filePath, ReadManyLines(fileNames));

        /// <summary>
        /// Returns the path in which the given directory path has been removed.
        /// </summary>
        public static string TrimDirectoryFromPath(string dir, string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (string.IsNullOrEmpty(dir)) return path;

            var dirFullPath = Path.GetFullPath(dir).ToLowerInvariant();
            var fullPath = Path.GetFullPath(path).ToLowerInvariant();

            return fullPath.StartsWith(dirFullPath)
                ? fullPath.Substring(dir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                : fullPath;
        }

        /// <summary>
        /// Reads all bytes from a stream
        /// https://stackoverflow.com/questions/1080442/how-to-convert-an-stream-into-a-byte-in-c
        /// </summary>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static FileStream OpenFileStreamWriting(string filePath, int bufferSize)
            => new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None, bufferSize);

        public static FileStream OpenFileStreamReading(string filePath, int bufferSize)
            => new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);

        /// <summary>
        /// The official Stream.Read iis a PITA, because it could return anywhere from 0 to the number of bytes
        /// requested, even in mid-stream. This call will read everything it can until it reaches
        /// the end of the stream of "count" bytes.
        /// </summary>
        public static int SafeRead(this Stream stream, byte[] buffer, int offset, int count)
        {
            var r = stream.Read(buffer, offset, count);
            if (r != 0 && r < count)
            {
                // We didn't read everything, so let's keep trying until we get a zero
                while (true)
                {
                    var tmp = stream.Read(buffer, r, count - r);
                    if (tmp == 0)
                        break;
                    r += tmp;
                }
            }

            return r;
        }


        /// <summary>
        /// Returns a binary writer for the given file path
        /// </summary>
        public static BinaryWriter CreateBinaryWriter(string filePath)
            => new BinaryWriter(File.OpenWrite(filePath));

        /// <summary>
        /// Returns a binary reader for the given file path
        /// </summary>
        public static BinaryReader CreateBinaryReader(string filePath)
            => new BinaryReader(File.OpenRead(filePath));

        /// <summary>
        /// Creates an empty file 
        /// </summary>
        public static void CreateEmptyFile(string filePath)
            => File.CreateText(filePath).Close();

        public static IEnumerable<FilePath> GetFiles(this DirectoryPath directoryPath, string searchPattern,
            bool recurse = false)
            => Directory.EnumerateFiles(directoryPath, searchPattern,
                recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(f => (FilePath)f);

        public static bool Exists(this FilePath filePath)
            => filePath.Info().Exists;

        public static FileInfo Info(this FilePath filePath)
            => new FileInfo(filePath);

        public static string ReadAllText(this FilePath self)
            => File.ReadAllText(self);

        public static byte[] ReadAllBytes(this FilePath self)
            => File.ReadAllBytes(self);

        public static string[] ReadAllLines(this FilePath self)
            => File.ReadAllLines(self);

        public static void Delete(this FilePath self)
            => File.Delete(self);

        public static void Create(this FilePath self)
            => File.Create(self);

        public static void CopyToStreamAndClose(this FilePath filePath, Stream outputStream)
        {
            if (filePath.Exists())
            {
                using (var inputStream = File.OpenRead(filePath))
                {
                    inputStream.CopyTo(outputStream);
                }
            }
        }
    }
}