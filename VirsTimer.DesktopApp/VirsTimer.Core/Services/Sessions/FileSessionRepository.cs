using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Helpers;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Cache;

namespace VirsTimer.Core.Services.Sessions
{
    /// <summary>
    /// <see cref="ISessionRepository"/> implementation that manages solves in local file.
    /// </summary>
    public class FileSessionRepository : ISessionRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly FileHelper _fileHelper;
        private readonly IApplicationCacheSaver _applicationCacheSaver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSessionRepository"/> class.
        /// </summary>
        public FileSessionRepository(IFileSystem fileSystem, FileHelper fileHelper, IApplicationCacheSaver applicationCacheSaver)
        {
            _fileSystem = fileSystem;
            _fileHelper = fileHelper;
            _applicationCacheSaver = applicationCacheSaver;
        }

        /// <summary>
        /// Gets sessions from local directory with <paramref name="event"/> id as name. 
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Session>>> GetSessionsAsync(Event @event)
        {
            var eventDirectory = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Id);

            var sessionsIds = _fileSystem.Directory.EnumerateFiles(eventDirectory)
                .Select(path => Regexes.JsonFile.Match(_fileSystem.Path.GetFileName(path)))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .ToList();

            var sessions = sessionsIds
                .Select(id => new Session(
                    @event,
                    id,
                    _applicationCacheSaver.ApplicationCache.SessionsByEvent[@event.Id][id]))
                .ToList();

            if (sessions.Count == 0)
            {
                var session = new Session(@event, "Sesja1");
                await AddSessionAsync(session).ConfigureAwait(false);
                sessions.Add(session);
            }

            await _applicationCacheSaver.UpdateCacheAsync().ConfigureAwait(false);
            return new RepositoryResponse<IReadOnlyList<Session>>(sessions);
        }

        /// <summary>
        /// Adds session in local directory.
        /// </summary>
        public async Task<RepositoryResponse> AddSessionAsync(Session session)
        {
            session.Id = Guid.NewGuid().ToString();

            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, session.Event.Id, $"{session.Id}{FileExtensions.Json}");
            await _fileHelper.WriteToFileIfNoExistsAsync(targetFile, Json.EmptyArray).ConfigureAwait(false);

            _applicationCacheSaver.ApplicationCache.SessionsByEvent[session.Event.Id].Add(session.Id, session.Name);
            await _applicationCacheSaver.UpdateCacheAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Updates session in local directory.
        /// </summary>
        public async Task<RepositoryResponse> UpdateSessionAsync(Session session)
        {
            _applicationCacheSaver.ApplicationCache.SessionsByEvent[session.Event.Id][session.Id] = session.Name;
            await _applicationCacheSaver.UpdateCacheAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Deletes session from local directory.
        /// </summary>
        public async Task<RepositoryResponse> DeleteSessionAsync(Session session)
        {
            var sourceFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, session.Event.Id, $"{session.Id}{FileExtensions.Json}");
            _fileSystem.File.Delete(sourceFile);

            _applicationCacheSaver.ApplicationCache.SessionsByEvent[session.Event.Id].Remove(session.Id);
            await _applicationCacheSaver.UpdateCacheAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }
    }
}
