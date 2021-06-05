using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

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
    }
}
