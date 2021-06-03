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
using VirsTimer.Core.Services.Sessions;

namespace VirsTimer.Core.Services
{
    public partial class FileSolvesService : IEventsGetter, ISessionsManager
    {
        private const string EmptyArrayJson = "[]";
        private static readonly Regex JsonFileRegex = new Regex("([^<>:\"/\\|?*]+)\\.json", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IFileSystem _fileSystem;

        public FileSolvesService(IFileSystem? fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public Task<IReadOnlyList<Event>> GetEventsAsync()
        {
            var events = new[]
            {
                new Event(Server.Events.ThreeByThree),
                new Event(Server.Events.TwoByTwo),
                new Event(Server.Events.FourByFour),
                new Event(Server.Events.FiveByFive),
                new Event(Server.Events.SixBySix),
                new Event(Server.Events.SevenBySeven),
                new Event(Server.Events.Megaminx),
                new Event(Server.Events.Pyraminx),
                new Event(Server.Events.Skewb),
                new Event(Server.Events.Square_One),
                new Event(Server.Events.Clock),
                new Event(Server.Events.ThreeByThreeOneHand),
                new Event(Server.Events.ThreeByThreeBlindfold),
                new Event(Server.Events.FourByFourBlindfold),
                new Event(Server.Events.FiveByFiveBlindfold),
            };
            return Task.FromResult<IReadOnlyList<Event>>(events);
        }

        public async Task<IReadOnlyList<Session>> GetAllSessionsAsync(Event @event)
        {
            var targetDirectory = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name);
            if (!_fileSystem.Directory.Exists(targetDirectory))
                _fileSystem.Directory.CreateDirectory(targetDirectory);
            var sessions = _fileSystem.Directory.EnumerateFiles(targetDirectory)
                .Select(path => JsonFileRegex.Match(_fileSystem.Path.GetFileName(path)))
                .Where(match => match.Success)
                .Select(match => new Session(match.Groups[1].Value))
                .ToList();

            if (sessions.Count == 0)
            {
                var session = await AddSessionAsync(@event, "Sesja1").ConfigureAwait(false);
                sessions = new List<Session> { session };
            }

            return sessions;
        }

        public async Task<IReadOnlyList<Solve>> GetSolvesAsync(Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            if (!_fileSystem.File.Exists(targetFile))
                return Array.Empty<Solve>();

            using var stream = _fileSystem.File.OpenRead(targetFile);
            var solves = await JsonSerializer.DeserializeAsync<IReadOnlyList<Solve>>(stream).ConfigureAwait(false) ?? Array.Empty<Solve>();
            await Task.WhenAll(
                solves.Select(solve => Task.Run(() =>
                {
                    solve.Event = @event;
                    solve.Session = session;
                })))
                .ConfigureAwait(false);

            return solves;
        }

        public Task<Session> AddSessionAsync(Event @event, string name)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{name}{FileExtensions.Json}");
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(targetFile));
            if (!_fileSystem.File.Exists(targetFile))
            {
                using var stream = _fileSystem.File.Create(targetFile);
                stream.Write(Encoding.UTF8.GetBytes(EmptyArrayJson), 0, EmptyArrayJson.Length);
            }
            var session = new Session(name);
            return Task.FromResult(session);
        }

        public Task<Session> RenameSessionAsync(Event @event, Session session, string newName)
        {
            var sourceFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{newName}{FileExtensions.Json}");
            _fileSystem.File.Move(sourceFile, targetFile);

            return Task.FromResult(new Session(session.Id, newName));
        }

        public Task DeleteSessionAsync(Event @event, Session session)
        {
            var sourceFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            _fileSystem.File.Delete(sourceFile);

            return Task.CompletedTask;
        }

    }
}
