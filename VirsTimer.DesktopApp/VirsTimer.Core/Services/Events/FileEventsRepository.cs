using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Events
{
    /// <summary>
    /// <see cref="IEventsRepository"/> implementation that manages events in local file.
    /// </summary>
    public class FileEventsRepository : IEventsRepository
    {
        /// <summary>
        /// Gets events from local file. 
        /// </summary>
        public Task<IReadOnlyList<Event>> GetEventsAsync()
        {
            var events = Server.Events.All.Select(x => new Event(x)).ToList();
            return Task.FromResult<IReadOnlyList<Event>>(events);
        }
    }
}
