using Nito.AsyncEx;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;

namespace VirsTimer.Core.Services.Cache
{
    /// <summary>
    /// <see cref="IApplicationCacheLoader"/> and <see cref="IApplicationCacheSaver"/> file handle implementation.
    /// </summary>
    public class ApplicationCacheFileIO : IApplicationCacheLoader, IApplicationCacheSaver
    {
        private const string DefaultFileName = "Cache.json";

        private readonly AsyncLock _muetx;
        private readonly IFileSystem _fileSystem;
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationCacheFileIO"/> class.
        /// </summary>
        public ApplicationCacheFileIO(IFileSystem fileSystem, string? fileName = null)
        {
            _muetx = new AsyncLock();
            _fileSystem = fileSystem;
            _filePath = _fileSystem.Path.Combine(Application.ApplicationDirectoryPath, fileName ?? DefaultFileName);
        }

        /// <summary>
        /// Saves <paramref name="applicationCache"/> to file.
        /// </summary>
        public async Task SaveCacheAsync(IApplicationCache applicationCache)
        {
            using var _ = await _muetx.LockAsync().ConfigureAwait(false);
            using var stream = _fileSystem.File.OpenWrite(_filePath);
            stream.SetLength(0);

            await JsonSerializer.SerializeAsync(stream, applicationCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads <see cref="IApplicationCache"/> from file.
        /// </summary>
        public async Task<IApplicationCache> LoadCacheAsync()
        {
            await _fileSystem.CreateNonexistentFileAsync(_filePath, Json.EmptyObject).ConfigureAwait(false);
            using var stream = _fileSystem.File.OpenRead(_filePath);

            return JsonSerializer.Deserialize<ApplicationCache>(stream) ?? new();
        }
    }
}