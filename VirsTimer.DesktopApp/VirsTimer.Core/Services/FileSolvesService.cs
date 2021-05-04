using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public partial class FileSolvesService : IPastSolvesGetter, ISolvesSaver, IEventsGetter
    {
        private readonly IFileSystem _fileSystem;

        public FileSolvesService(IFileSystem? fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public Task<IReadOnlyList<Event>> GetEventsAsync()
        {
            return Task.FromResult<IReadOnlyList<Event>>(new[] { new Event("3x3x3") });
        }

        public async Task<IReadOnlyList<Solve>> GetSolvesAsync(string @event, string session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.CommonDirectoryPath, @event, $"{session}{FileExtensions.Json}");
            if (!_fileSystem.File.Exists(targetFile))
                return Array.Empty<Solve>();

            using var stream = _fileSystem.File.OpenRead(targetFile);
            var solves = await JsonSerializer.DeserializeAsync<SolvesCollection>(stream).ConfigureAwait(false);

            return solves;
        }

        public async Task SaveSolvesAsync(IEnumerable<Solve> solves, string @event, string session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.CommonDirectoryPath, @event, $"{session}{FileExtensions.Json}");
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(targetFile));
            using var stream = _fileSystem.File.Exists(targetFile) ? _fileSystem.File.Create(targetFile) : _fileSystem.File.OpenWrite(targetFile);
            await JsonSerializer.SerializeAsync(stream, solves).ConfigureAwait(false);
        }
    }
}
