using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Events
{
    /// <summary>
    /// Service providing repository operations for <see cref="Event"/> model.
    /// </summary>
    public interface IEventsRepository
    {
        /// <summary>
        /// Gets solves from repository. 
        /// </summary>
        Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync();

        /// <summary>
        /// Adds event to repository.
        /// </summary>
        Task<RepositoryResponse> AddEventAsync(Event @event);

        /// <summary>
        /// Update event in repository.
        /// </summary>
        Task<RepositoryResponse> UpdateEventAsync(Event @event);

        /// <summary>
        /// Deletes event from repository.
        /// </summary>
        Task<RepositoryResponse> DeleteEventAsync(Event @event);
    }
}