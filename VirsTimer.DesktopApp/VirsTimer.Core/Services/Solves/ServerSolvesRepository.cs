using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Solves
{
    /// <summary>
    /// Virs timer server api <see cref="ServerSolvesRepository"/> implementation. 
    /// </summary>
    public class ServerSolvesRepository : AbstractServerRepository, ISolvesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSolvesRepository"/> class.
        /// </summary>
        public ServerSolvesRepository(
            IHttpClientFactory httpClientFactory,
            IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> AddSolveAsync(Solve solve)
        {
            try
            {
                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Solve.Post;

                var content = new SolvePostRequest(UserClient.Id, solve);
                var httpResponse = await client.PostAsJsonAsync(endpoint, content).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync<SolvePostResponse>(httpResponse).ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                    solve.Id = response.Value.Id;

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteSolveAsync(Solve solve)
        {
            try
            {
                if (solve.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Solve.Delete(solve.Id);
                var httpResponse = await client.DeleteAsync(endpoint).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync(httpResponse).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Solve>>> GetSolvesAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Solve.GetBySession(session.Id);
                var solveResponse = await client.GetFromJsonAsync<SolveGetRequest[]>(endpoint).ConfigureAwait(false) ?? Array.Empty<SolveGetRequest>();
                var solves = solveResponse.Select(x => x.ToSolve(session)).ToList();

                return new RepositoryResponse<IReadOnlyList<Solve>>(solves);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IReadOnlyList<Solve>>(ex.StatusCode!, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateSolveAsync(Solve solve)
        {
            try
            {
                if (solve.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Solve.Patch(solve.Id);

                var request = new SolvePatchRequest(solve)
                var httpResponse = await client.PatchAsJsonAsync(endpoint, request).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync(httpResponse).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }
    }
}