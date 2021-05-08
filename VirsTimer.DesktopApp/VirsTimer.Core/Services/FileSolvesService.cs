using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public partial class FileSolvesService : IPastSolvesGetter, ISolvesSaver, IEventsGetter, ISessionsManager
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

        private static readonly Regex JsonFileRegex = new Regex("([^<>:\"/\\|?*]+)\\.json", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public Task<IReadOnlyList<Session>> GetSessionsAsync(Event @event)
        {
            var targetDirectory = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name);
            var sessions = _fileSystem.Directory.EnumerateFiles(targetDirectory)
                .Select(path => JsonFileRegex.Match(_fileSystem.Path.GetFileName(path)))
                .Where(match => match.Success)
                .Select(match => new Session(match.Groups[1].Value))
                .ToList();

            return Task.FromResult<IReadOnlyList<Session>>(sessions);
        }

        public async Task<IReadOnlyList<Solve>> GetSolvesAsync(Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            if (!_fileSystem.File.Exists(targetFile))
                return Array.Empty<Solve>();

            using var stream = _fileSystem.File.OpenRead(targetFile);
            var solves = await JsonSerializer.DeserializeAsync<SolvesCollection>(stream).ConfigureAwait(false);

            return solves;
        }

        public async Task SaveSolvesAsync(IEnumerable<Solve> solves, Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(targetFile));
            using var stream = _fileSystem.File.Exists(targetFile) ? _fileSystem.File.Create(targetFile) : _fileSystem.File.OpenWrite(targetFile);
            await JsonSerializer.SerializeAsync(stream, solves).ConfigureAwait(false);
        }

        public Task AddSessionAsync(Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(targetFile));
            if (!_fileSystem.File.Exists(targetFile))
                _fileSystem.File.Create(targetFile);

            return Task.CompletedTask;
        }
    }
}
