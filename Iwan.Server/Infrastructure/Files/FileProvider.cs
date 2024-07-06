using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Options;
using ChinhDo.Transactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.Files
{
    [Injected(ServiceLifetime.Scoped, typeof(IFileProvider))]
    public class FileProvider : IFileProvider
    {
        private static IFileManager TxFileManagerInstance { get; } = new TxFileManager();
        protected readonly string _webrootPath;

        public FileProvider(PathsOptions pathsOptions)
        {
            _webrootPath = pathsOptions.WebRootPath;
        }

        #region Utilities

        /// <summary>
        /// Determines if the string is a valid Universal Naming Convention (UNC)
        /// for a server and share path.
        /// </summary>
        /// <param name="path">The path to be tested.</param>
        /// <returns><see langword="true"/> if the path is a valid UNC path; 
        /// otherwise, <see langword="false"/>.</returns>
        protected static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
        }

        #endregion

        /// <summary>
        /// Combines an array of strings into a path
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        public virtual string Combine(params string[] paths)
        {
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
                //add leading slash to correctly form path in the UNIX system
                path = "/" + path;

            return path;
        }


        /// <summary>
        /// Combines an array of strings into a path while adding the root path at the beginning
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        public string CombineWithRoot(params string[] paths)
        {
            var arrayWithRoot = new string[paths.Length + 1];
            Array.Copy(paths, 0, arrayWithRoot, 1, paths.Length);
            arrayWithRoot[0] = _webrootPath;
            return Combine(arrayWithRoot);
        }

        /// <summary>
        /// Returns the absolute path to the directory
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The absolute path to the directory</returns>
        public virtual string GetAbsolutePath(params string[] paths)
        {
            var allPaths = new List<string>();

            if (paths.Any() && !paths[0].Contains(_webrootPath, StringComparison.InvariantCulture))
                allPaths.Add(_webrootPath);

            allPaths.AddRange(paths);

            return Combine(allPaths.ToArray());
        }

        /// <summary>
        /// Deletes the specified file
        /// </summary>
        /// <param name="filePath">The name of the file to be deleted. Wildcard characters are not supported</param>
        public virtual void DeleteFile(string filePath, bool useTxFileManager = false)
        {
            if (!FileExists(filePath))
                return;

            if (useTxFileManager)
                TxFileManagerInstance.Delete(filePath);
            else
                File.Delete(filePath);
        }

        public void DeleteFiles(IEnumerable<string> filePaths, bool useTxFileManager = false)
        {
            foreach (var filePath in filePaths) DeleteFile(filePath, useTxFileManager);
        }

        /// <summary>
        /// Determines whether the specified file exists
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        public virtual bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException) { return true; }

            return false;
        }

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist
        /// </summary>
        /// <param name="path">The directory to create</param>
        public virtual void CreateDirectory(string path, bool useTxFileManager = false)
        {
            if (DirectoryExists(path))
                return;

            if (useTxFileManager)
                TxFileManagerInstance.CreateDirectory(path);
            else
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// true if path refers to an existing directory; false if the directory does not exist or an error occurs when
        /// trying to determine if the specified file exists
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Forms a full path using the passed info
        /// </summary>
        /// <param name="rootPath">Path to root directory</param>
        /// <param name="virtualPath">Virtual path to the related diretory</param>
        /// <param name="postfix">Postfix to be added to the filename</param>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The formed full path as the first returned value and the filename as the second returned value</returns>
        public (string, string) FormNewFilePath(string virtualPath, string postfix = "", string extension = ".jpg")
        {
            var fileGuid = Guid.NewGuid().ToString();
            var fileName = $"{fileGuid}_{postfix}{extension}";
            var fullPath = Combine(_webrootPath, virtualPath, fileName);

            return (fullPath, GetFileName(fullPath));
        }

        /// <summary>
        /// Returns the extension of the specified path string
        /// </summary>
        /// <param name="filePath">The path string from which to get the extension</param>
        /// <returns>The extension of the specified path (including the period ".")</returns>
        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// Returns the file name and extension of the specified path string
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension</param>
        /// <returns>The characters after the last directory character in path</returns>
        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Returns the file name of the specified path string without the extension
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The file name, minus the last period (.) and all characters following it</returns>
        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path</param>
        /// <param name="destFileName">The new path and name for the file</param>
        public virtual void MoveFile(string sourceFileName, string destFileName, bool useTxFileManager = false)
        {
            if (!FileExists(sourceFileName))
                throw new FileNotFoundException($"{sourceFileName} file could not be found");

            if (FileExists(destFileName))
                throw new FileNotFoundException($"{destFileName} file could not be found");

            if (useTxFileManager)
                TxFileManagerInstance.Move(sourceFileName, destFileName);
            else
                File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// Moves a list of sources and destinations files
        /// </summary>
        public void MoveFiles(IEnumerable<(string sourceFileName, string destFileName)> movements, bool useTxFileManager = false)
        {
            foreach (var (sourceFileName, destFileName) in movements) MoveFile(sourceFileName, destFileName, useTxFileManager);
        }

        /// <summary>
        /// Reads the contents of the file into a byte array
        /// </summary>
        /// <param name="filePath">The file for reading</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains a byte array containing the contents of the file
        /// </returns>
        public virtual async Task<byte[]> ReadAllBytesAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (!FileExists(filePath))
                throw new FileNotFoundException($"{filePath} file could not be found");

            return await File.ReadAllBytesAsync(filePath, cancellationToken);
        }

        /// <summary>
        /// Writes the specified byte array to the file
        /// </summary>
        /// <param name="filePath">The file to write to</param>
        /// <param name="bytes">The bytes to write to the file</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task WriteAllBytesAsync(string filePath, byte[] bytes, bool useTxFileManager = false, CancellationToken cancellationToken = default)
        {
            if (useTxFileManager)
                TxFileManagerInstance.WriteAllBytes(filePath, bytes);
            else
                await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
        }

        /// <summary>
        /// Copies data from the passed stream to the passed file path
        /// </summary>
        public async Task WriteAllBytesAsync(Stream readStream, long length, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default)
        {
            var bytes = new byte[length];
            await readStream.ReadAsync(bytes, 0, (int)length, cancellationToken);
            await WriteAllBytesAsync(filePath, bytes, useTxFileManager, cancellationToken);
        }

        /// <summary>
        /// Copies data from the passed copying action to the passed file path
        /// </summary>
        public async Task WriteAllBytesAsync(Func<Stream, CancellationToken, Task> copyingAction, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default)
        {
            if (useTxFileManager)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await copyingAction.Invoke(memoryStream, cancellationToken);
                    await WriteAllBytesAsync(filePath, memoryStream.ToArray(), useTxFileManager, cancellationToken);
                }
            }
            else
                using (var fileStream = File.Exists(filePath) ? File.OpenWrite(filePath) : File.Create(filePath))
                    await copyingAction.Invoke(fileStream, cancellationToken);
        }

        /// <summary>
        /// Copies data from the passed copying action to the passed file path
        /// </summary>
        public async Task WriteAllBytesAsync(Func<Stream, Task> copyingAction, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default)
        {
            if (useTxFileManager)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await copyingAction.Invoke(memoryStream);
                    await WriteAllBytesAsync(filePath, memoryStream.ToArray(), useTxFileManager, cancellationToken);
                }
            }
            else
                using (var fileStream = File.Exists(filePath) ? File.OpenWrite(filePath) : File.Create(filePath))
                    await copyingAction.Invoke(fileStream);
        }
    }
}
