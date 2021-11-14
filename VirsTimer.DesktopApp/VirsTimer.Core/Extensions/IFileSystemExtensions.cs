using System;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;

namespace VirsTimer.Core.Extensions
{
    /// <summary>
    /// Provides extension for <see cref="IFileSystem"/>.
    /// </summary>
    public static class IFileSystemExtensions
    {
        /// <summary>
        /// Creates file with <paramref name="content"/> only if file in <paramref name="path"/> does not exists.
        /// </summary>
        public static async Task CreateNonexistentFileAsync(this IFileSystem fileSystem, string path, string content)
        {
            fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(path));
            if (fileSystem.File.Exists(path))
                return;

            using var stream = fileSystem.File.Create(path);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(content).AsMemory(0, content.Length));
        }
    }
}