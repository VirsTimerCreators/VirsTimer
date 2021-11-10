using System;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;

namespace VirsTimer.Core.Helpers
{
    /// <summary>
    /// Helper for directories/files operations.
    /// </summary>
    public class FileHelper
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHelper"/> class.
        /// </summary>
        public FileHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Writes content to file if file does not exists.
        /// </summary>
        public void WriteToFileIfNoExists(string path, string content)
        {
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(path));
            if (!_fileSystem.File.Exists(path))
            {
                using var stream = _fileSystem.File.Create(path);
                stream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
            }
        }

        /// <summary>
        /// Writes content to file if file does not exists asynchonously.
        /// </summary>
        public async Task WriteToFileIfNoExistsAsync(string path, string content)
        {
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(path));
            if (!_fileSystem.File.Exists(path))
            {
                using var stream = _fileSystem.File.Create(path);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(content).AsMemory(0, content.Length));
            }
        }
    }
}