using Nito.AsyncEx;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Helpers;

namespace VirsTimer.Core.Services.Cache
{
    public class ApplicationCacheSaver : IApplicationCacheSaver
    {
        private const string FileName = "Cache.json";

        private readonly AsyncLock _muetx = new AsyncLock();
        private readonly IFileSystem _fileSystem;
        private readonly FileHelper _fileHelper;
        private readonly string _filePath;

        public IApplicationCache ApplicationCache { get; }

        public ApplicationCacheSaver(IFileSystem fileSystem, FileHelper fileHelper)
        {
            _fileSystem = fileSystem;
            _fileHelper = fileHelper;
            _filePath = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, FileName);
            ApplicationCache = LoadFromFile();
        }

        private IApplicationCache LoadFromFile()
        {
            _fileHelper.WriteToFileIfNoExists(_filePath, Json.EmptyObject);
            var cache = _fileSystem.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<ApplicationCache>(cache) ?? new();
        }

        public async Task UpdateCacheAsync()
        {
            using (await _muetx.LockAsync().ConfigureAwait(false))
            {
                using var stream = _fileSystem.File.OpenWrite(_filePath);
                await JsonSerializer.SerializeAsync(stream, ApplicationCache).ConfigureAwait(false);
            }
        }
    }
}
