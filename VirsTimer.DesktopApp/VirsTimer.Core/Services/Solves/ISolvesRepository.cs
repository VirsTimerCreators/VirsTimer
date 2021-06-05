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
        /// Gets solves from repository by given <paramref name="session"/>. 
        /// </summary>
        Task<RepositoryResponse<IReadOnlyList<Solve>>> GetSolvesAsync(Session session);

        /// <summary>
        /// Saves solve in repository.
        /// </summary>
        Task<RepositoryResponse> AddSolveAsync(Solve solve);

        /// <summary>
        /// Updates solve in repository.
        /// </summary>
        Task<RepositoryResponse> UpdateSolveAsync(Solve solve);

        /// <summary>
        /// Deletes solve from repository.
        /// </summary>
        Task<RepositoryResponse> DeleteSolveAsync(Solve solve);
    }
}
