using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;

namespace VirsTimer.Core.Services.Solves
{
    public class ServerSolvesRepository : AbstractServerRepository, ISolvesRepository
    {
        public ServerSolvesRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        public async Task<RepositoryResponse> AddSolveAsync(Solve solve)
        {
            try
            {
                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Solves);

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

        public async Task<RepositoryResponse> DeleteSolveAsync(Solve solve)
        {
            try
            {
                if (solve.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Solves, solve.Id);
                var httpResponse = await client.DeleteAsync(endpoint).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync(httpResponse).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse<IReadOnlyList<Solve>>> GetSolvesAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Solves, Server.Endpoints.Sessions, session.Id);
                var solveResponse = await client.GetFromJsonAsync<SolveGetRequest[]>(endpoint).ConfigureAwait(false) ?? Array.Empty<SolveGetRequest>();
                var solves = solveResponse.Select(x => x.ToSolve(session)).ToList();

                return new RepositoryResponse<IReadOnlyList<Solve>>(solves);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IReadOnlyList<Solve>>((HttpStatusCode)ex.StatusCode!, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IReadOnlyList<Solve>>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse> UpdateSolveAsync(Solve solve)
        {
            try
            {
                if (solve.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Solve Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Solves, solve.Id);

                using var content = CreateJsonRequest(new SolvePatchRequest(solve));
                var httpResponse = await client.PatchAsync(endpoint, content).ConfigureAwait(false);
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
