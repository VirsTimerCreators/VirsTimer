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
    /// <see cref="IEventsRepository"/> implementation that manages events in local file.
    /// </summary>
    public class FileEventsRepository : IEventsRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IApplicationCacheSaver _applicationCacheSaver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solve"/> class.
        /// </summary>
        public FileEventsRepository(IFileSystem fileSystem, IApplicationCacheSaver applicationCacheSaver)
        {
            _fileSystem = fileSystem;
            _applicationCacheSaver = applicationCacheSaver;
        }

        /// <summary>
        /// Gets events from local directories. 
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync()
        {
            var eventsNames = _applicationCacheSaver.ApplicationCache.EventsNames;
            if (eventsNames.Count > 0)
                return new RepositoryResponse<IReadOnlyList<Event>>(eventsNames.Select(x => new Event(x.Key, x.Value)).ToList());

            var events = Constants.Events.All.Select(x => new Event(Guid.NewGuid().ToString(), x)).ToList();
            foreach (var @event in events)
            {
                _applicationCacheSaver.ApplicationCache.EventsNames.Add(@event.Id, @event.Name);
                _applicationCacheSaver.ApplicationCache.SessionsByEvent.Add(@event.Id, new Dictionary<string, string>());
            }

            var directoriesTasks = events.Select(x =>
            {
                var targerDirectory = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, x.Id);
                return Task.Run(() => _fileSystem.Directory.CreateDirectory(targerDirectory));
            });

            await Task.WhenAll(directoriesTasks).ConfigureAwait(false);
            await _applicationCacheSaver.UpdateCacheAsync().ConfigureAwait(false);
            return new RepositoryResponse<IReadOnlyList<Event>>(events);
        }
    }
}
