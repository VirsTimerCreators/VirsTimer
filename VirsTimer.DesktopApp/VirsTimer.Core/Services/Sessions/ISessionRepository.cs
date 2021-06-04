using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Sessions
{
    /// <summary>
    /// Service providing repository operations for <see cref="Session"/> model.
    /// </summary>
    public interface ISessionRepository
    {
        /// <summary>
        /// Gets sessions from repository by given <paramref name="event"/>. 
        /// </summary>
        Task<IReadOnlyList<Session>> GetSessionsAsync(Event @event);

        /// <summary>
        /// Adds session in repository.
        /// </summary>
        Task AddSessionAsync(Session session);

        /// <summary>
        /// Updates session in repository.
        /// </summary>
        Task UpdateSessionAsync(Session session);

        /// <summary>
        /// Deletes session from repository.
        /// </summary>
        Task DeleteSessionAsync(Session session);
    }
}
