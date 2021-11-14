using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Solves
{
    /// <summary>
    /// <see cref="ISolvesRepository"/> implementation that manages solves in local file.
    /// </summary>
    public partial class SolvesFileRepository : ISolvesRepository
    {
        private readonly Cache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolvesFileRepository"/> class.
        /// </summary>
        /// <param name="fileSystem">File system.</param>
        public SolvesFileRepository(IFileSystem fileSystem)
        {
            _cache = new Cache(string.Empty, fileSystem);
        }

        /// <summary>
        /// Gets solves from local file by given <paramref name="session"/> and <paramref name="session"/>. 
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Solve>>> GetSolvesAsync(Session session)
        {
            if (session.Id is null)
                return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            await _cache.RefreshCacheAsync(session).ConfigureAwait(false);
            return new RepositoryResponse<IReadOnlyList<Solve>>(_cache.LoadedSolves);
        }

        /// <summary>
        /// Saves solve in local file.
        /// </summary>
        public async Task<RepositoryResponse> AddSolveAsync(Solve solve)
        {
            if (solve.Session?.Id is null)
                return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.ClientError, "Solve session Id cannot be null.");

            solve.Id ??= Guid.NewGuid().ToString();

            await _cache.RefreshCacheAsync(solve.Session).ConfigureAwait(false);
            _cache.LoadedSolves.Add(solve);
            await _cache.UpdateCacheTargetAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Updates solve in local file.
        /// </summary>
        public async Task<RepositoryResponse> UpdateSolveAsync(Solve solve)
        {
            await _cache.RefreshCacheAsync(solve.Session).ConfigureAwait(false);
            var foundSolve = _cache.LoadedSolves.Find(x => x.Id == solve.Id);
            if (foundSolve == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Couldn't update solve, cause it wasn't in FileSoveRepository.");

            foundSolve.Flag = solve.Flag;
            await _cache.UpdateCacheTargetAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// Deletes solve from local file.
        /// </summary>
        public async Task<RepositoryResponse> DeleteSolveAsync(Solve solve)
        {
            await _cache.RefreshCacheAsync(solve.Session).ConfigureAwait(false);
            var foundSolve = _cache.LoadedSolves.Find(x => x.Id == solve.Id);
            if (foundSolve == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Couldn't update solve, cause it wasn't in FileSoveRepository.");

            _cache.LoadedSolves.Remove(foundSolve);
            await _cache.UpdateCacheTargetAsync().ConfigureAwait(false);

            return RepositoryResponse.Ok;
        }
    }
}