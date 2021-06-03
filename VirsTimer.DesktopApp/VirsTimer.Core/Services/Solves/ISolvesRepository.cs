using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Solves
{
    /// <summary>
    /// Service providing repository operations for <see cref="Solve"/> model.
    /// </summary>
    public interface ISolvesRepository
    {
        /// <summary>
        /// Gets solves from repository by given <paramref name="event"/> and <paramref name="session"/>. 
        /// </summary>
        Task<IReadOnlyList<Solve>> GetSolvesAsync(Event @event, Session session);

        /// <summary>
        /// Saves solve in repository.
        /// </summary>
        Task SaveSolveAsync(Solve solve);

        /// <summary>
        /// Updates solve in repository.
        /// </summary>
        Task UpdateSolveAsync(Solve solve);

        /// <summary>
        /// Deletes solve from repository.
        /// </summary>
        Task DeleteSolveAsync(Solve solve);
    }
}
