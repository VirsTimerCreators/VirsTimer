using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Solves
{
    /// <summary>
    /// Virs timer server api <see cref="SolvesServerRepository"/> implementation. 
    /// </summary>
    public class SolvesServerRepository : ISolvesRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserClient _userClient;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolvesServerRepository"/> class.
        /// </summary>
        public SolvesServerRepository(
            IHttpClientFactory httpClientFactory,
            IUserClient userClient,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _userClient = userClient;
            _httpResponseHandler = httpResponseHandler;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> AddSolveAsync(Solve solve)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new SolvePostRequest(_userClient.Id, solve);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Solve.Post, request);
            var response = await _httpResponseHandler.HandleAsync<SolvePostResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
                solve.Id = response.Value.Id;

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteSolveAsync(Solve solve)
        {
            if (solve.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Solve.Delete(solve.Id);
            var httpRequestFunc = () => client.DeleteAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync(httpRequestFunc).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Solve>>> GetSolvesAsync(Session session)
        {
            if (session.Id == null)
                return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Solve.GetBySession(session.Id);
            var httpRequestFunc = () => client.GetAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync<SolveGetRequest[]>(httpRequestFunc).ConfigureAwait(false);
            var solves = response.Value is not null
                ? response.Value.Select(x => x.ToSolve(session)).ToArray()
                : Array.Empty<Solve>();

            return new RepositoryResponse<IReadOnlyList<Solve>>(response, solves);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateSolveAsync(Solve solve)
        {
            if (solve.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Solve.Patch(solve.Id);

            var request = new SolvePatchRequest(solve);
            var httpRequestFunc = () => client.PatchAsJsonAsync(endpoint, request);
            var response = await _httpResponseHandler.HandleAsync(httpRequestFunc).ConfigureAwait(false);

            return response;
        }
    }
}