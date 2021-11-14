using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Cache;

namespace VirsTimer.Core.Services.Events
{
    /// <summary>
    /// <see cref="IEventsRepository"/> implementation that manages events in local files.
    /// </summary>
    public class EventsFileRepository : IEventsRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IApplicationCache _applicationCache;
        private readonly IApplicationCacheSaver _applicationCacheSaver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solve"/> class.
        /// </summary>
        public EventsFileRepository(
            IFileSystem fileSystem,
            IApplicationCache applicationCache,
            IApplicationCacheSaver applicationCacheSaver)
        {
            _fileSystem = fileSystem;
            _applicationCache = applicationCache;
            _applicationCacheSaver = applicationCacheSaver;
        }

        /// <summary>
        /// Gets events from local directories. 
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync()
        {
            if (_applicationCache.EventsById.Count > 0)
            {
                var eventsFromCache = _applicationCache.EventsById.Select(x => new Event(x.Key, x.Value));
                return new RepositoryResponse<IReadOnlyList<Event>>(eventsFromCache.ToList());
            }

            var predefinedEventsTasks = Constants.Events.Predefined.Select(x =>
            {
                var @event = new Event(x);
                return AddEventAsync(@event, saveCache: false);
            });

            var predefinedEvents = await Task.WhenAll(predefinedEventsTasks).ConfigureAwait(false);

            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);
            return new RepositoryResponse<IReadOnlyList<Event>>(predefinedEvents);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<Event>> AddEventAsync(Event @event)
        {
            await AddEventAsync(@event, saveCache: false);
            return new RepositoryResponse<Event>(@event);
        }

        private async Task<Event> AddEventAsync(Event @event, bool saveCache)
        {
            @event.Id ??= Guid.NewGuid().ToString();
            _applicationCache.EventsById.Add(@event.Id, @event.Name);
            _applicationCache.SessionsByEventId.Add(@event.Id, new Dictionary<string, string>());

            var targerDirectory = _fileSystem.Path.Combine(Application.ApplicationDirectoryPath, @event.Id);
            _fileSystem.Directory.CreateDirectory(targerDirectory);

            if (!saveCache)
                return @event;
            
            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);
            return @event;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateEventAsync(Event @event)
        {
            if (@event.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            _applicationCache.EventsById[@event.Id] = @event.Name;
            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteEventAsync(Event @event)
        {
            if (@event.Id is null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            _applicationCache.EventsById.Remove(@event.Id);
            _applicationCache.SessionsByEventId.Remove(@event.Id);

            await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }
    }
}