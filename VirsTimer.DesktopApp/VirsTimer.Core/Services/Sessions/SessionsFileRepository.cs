using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Cache;

namespace VirsTimer.Core.Services.Sessions
{
    /// <summary>
    /// <see cref="ISessionsRepository"/> implementation that manages solves in local file.
    /// </summary>
    public class SessionsFileRepository : ISessionsRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IApplicationCache _applicationCache;
        private readonly IApplicationCacheSaver _applicationCacheSaver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionsFileRepository"/> class.
        /// </summary>
        public SessionsFileRepository(
            IFileSystem fileSystem,
            IApplicationCache applicationCache,
            IApplicationCacheSaver applicationCacheSaver)
        {
            _fileSystem = fileSystem;
            _applicationCache = applicationCache;
            _applicationCacheSaver = applicationCacheSaver;
        }

        /// <summary>
        /// Gets sessions from local directory with <paramref name="event"/> id as name. 
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Session>>> GetSessionsAsync(Event @event)
        {
            if (@event.Id is null)
                return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            var eventDirectory = _fileSystem.Path.Combine(Application.ApplicationDirectoryPath, @event.Id);

            var sessions = _applicationCache.SessionsByEventId[@event.Id]
                .Select(sessionById => new Session(
                    @event,
                    sessionById.Key,
                    sessionById.Value))
                .ToList();

            if (sessions.Count > 0)
                return new RepositoryResponse<IReadOnlyList<Session>>(sessions);

            var newSession = new Session(@event, "Sesja1");
            await AddSessionAsync(newSession).ConfigureAwait(false);
            sessions.Add(newSession);

            return new RepositoryResponse<IReadOnlyList<Session>>(sessions);
        }

        /// <summary>
        /// Adds session in local directory.
        /// </summary>
        public async Task<RepositoryResponse> AddSessionAsync(Session session)
        {
            if (session.Event?.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session event Id cannot be null.");

            session.Id ??= Guid.NewGuid().ToString();

            var targetFile = GetSessionFile(session);
            await _fileSystem.CreateNonexistentFileAsync(targetFile, Json.EmptyArray).ConfigureAwait(false);

            _applicationCache.SessionsByEventId[session.Event.Id].Add(session.Id, session.Name);
            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Updates session in local directory.
        /// </summary>
        public async Task<RepositoryResponse> UpdateSessionAsync(Session session)
        {
            if (session.Event.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session event Id cannot be null.");

            if (session.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            _applicationCache.SessionsByEventId[session.Event.Id][session.Id] = session.Name;
            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Deletes session from local directory.
        /// </summary>
        public async Task<RepositoryResponse> DeleteSessionAsync(Session session)
        {
            if (session.Event.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session event Id cannot be null.");

            if (session.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            var sourceFile = GetSessionFile(session);
            _fileSystem.File.Delete(sourceFile);

            _applicationCache.SessionsByEventId[session.Event.Id].Remove(session.Id);
            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        private string GetSessionFile(Session session)
        {
            return _fileSystem.Path.Combine(Application.ApplicationDirectoryPath, session.Event.Id, $"{session.Id}{FileExtensions.Json}");
        }
    }
}