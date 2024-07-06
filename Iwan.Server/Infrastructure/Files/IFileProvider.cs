using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.Files
{
    public interface IFileProvider
    {
        /// <summary>
        /// Combines an array of strings into a path
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        string Combine(params string[] paths);

        /// <summary>
        /// Combines an array of strings into a path while adding the root path at the beginning
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        string CombineWithRoot(params string[] paths);

        /// <summary>
        /// Deletes the specified file
        /// </summary>
        /// <param name="filePath">The name of the file to be deleted. Wildcard characters are not supported</param>
        void DeleteFile(string filePath, bool useTxFileManager = false);

        /// <summary>
        /// Deletes the specified files
        /// </summary>
        /// <param name="filePaths">The name of the files to be deleted. Wildcard characters are not supported</param>
        void DeleteFiles(IEnumerable<string> filePaths, bool useTxFileManager = false);

        /// <summary>
        /// Returns the absolute path to the directory
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The absolute path to the directory</returns>
        string GetAbsolutePath(params string[] paths);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist
        /// </summary>
        /// <param name="path">The directory to create</param>
        void CreateDirectory(string path, bool useTxFileManager = false);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// true if path refers to an existing directory; false if the directory does not exist or an error occurs when
        /// trying to determine if the specified file exists
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path</param>
        /// <param name="destFileName">The new path and name for the file</param>
        void MoveFile(string sourceFileName, string destFileName, bool useTxFileManager = false);

        /// <summary>
        /// Moves a list of sources and destinations files
        /// </summary>
        void MoveFiles(IEnumerable<(string sourceFileName, string destFileName)> movementsTuple, bool useTxFileManager = false);

        /// <summary>
        /// Determines whether the specified file exists
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Checks if the file is locked by another thread
        /// </summary>
        /// <param name="filePath">File to check</param>
        /// <returns>True if locked and false otherwise</returns>
        bool IsFileLocked(string filePath);

        /// <summary>
        /// Returns the extension of the specified path string
        /// </summary>
        /// <param name="filePath">The path string from which to get the extension</param>
        /// <returns>The extension of the specified path (including the period ".")</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        /// Returns the file name and extension of the specified path string
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension</param>
        /// <returns>The characters after the last directory character in path</returns>
        string GetFileName(string path);

        /// <summary>
        /// Forms a full path using the passed info
        /// </summary>
        /// <param name="rootPath">Path to root directory</param>
        /// <param name="virtualPath">Virtual path to the related directory</param>
        /// <param name="postfix">Postfix to be added to the filename</param>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The formed full path as the first returned value and the filename as the second returned value</returns>
        (string, string) FormNewFilePath(string virtualPath, string postfix = "", string extension = ".jpg");

        /// <summary>
        /// Returns the file name of the specified path string without the extension
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The file name, minus the last period (.) and all characters following it</returns>
        string GetFileNameWithoutExtension(string filePath);

        /// <summary>
        /// Reads the contents of the file into a byte array
        /// </summary>
        /// <param name="filePath">The file for reading</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains a byte array containing the contents of the file
        /// </returns>
        Task<byte[]> ReadAllBytesAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Writes the specified byte array to the file
        /// </summary>
        /// <param name="filePath">The file to write to</param>
        /// <param name="bytes">The bytes to write to the file</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task WriteAllBytesAsync(string filePath, byte[] bytes, bool useTxFileManager = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies data from the passed copying action to the passed file path
        /// </summary>
        Task WriteAllBytesAsync(Func<Stream, Task> copyingAction, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Copies data from the passed copying action to the passed file path
        /// </summary>
        Task WriteAllBytesAsync(Func<Stream, CancellationToken, Task> copyingAction, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads bytes from the passed stream and writes bytes to the passed file path
        /// </summary>
        /// <param name="readStream">The stream to read the source data from</param>
        /// <param name="length">The length of the data to be read</param>
        /// <param name="filePath">The path of the file to be written on</param>
        /// <param name="cancellationToken">Token for signaling cancellation</param>
        /// <returns>A task that represents teh asynchronous operation</returns>
        Task WriteAllBytesAsync(Stream readStream, long length, string filePath, bool useTxFileManager = false, CancellationToken cancellationToken = default);
    }
}
