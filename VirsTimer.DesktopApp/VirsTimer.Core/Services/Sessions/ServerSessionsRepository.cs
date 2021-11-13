using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Sessions
{
    /// <summary>
    /// Virs timer server api <see cref="ServerSessionsRepository"/> implementation. 
    /// </summary>
    public class ServerSessionsRepository : AbstractServerRepository, ISessionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSessionsRepository"/> class.
        /// </summary>
        public ServerSessionsRepository(
            IHttpClientFactory httpClientFactory,
            IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> AddSessionAsync(Session session)
        {
            try
            {
                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);

                var content = new SessionPostRequest(UserClient.Id, session);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var httpResponse = await client.PostAsJsonAsync(Server.Endpoints.Session.Post, content, options).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync<SessionPostResponse>(httpResponse).ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                    session.Id = response.Value.Id;

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
        public async Task<RepositoryResponse> DeleteSessionAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Session.Delete(session.Id);
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
        public async Task<RepositoryResponse<IReadOnlyList<Session>>> GetSessionsAsync(Event @event)
        {
            try
            {
                if (@event.Id == null)
                    return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Session.GetByEvent(@event.Id);
                var sessionsResponse = await client.GetFromJsonAsync<SessionGetRequest[]>(endpoint).ConfigureAwait(false);

                if (sessionsResponse.IsNullOrEmpty())
                {
                    var session = new Session(@event, "Sesja1");
                    var sessionResponse = await AddSessionAsync(session).ConfigureAwait(false);
                    return new RepositoryResponse<IReadOnlyList<Session>>(new[] { session });
                }
                var sessions = sessionsResponse!.Select(x => x.ToSession(@event)).ToList();

                return new RepositoryResponse<IReadOnlyList<Session>>(sessions);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IReadOnlyList<Session>>(ex.StatusCode!, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateSessionAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                var client = HttpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
                var endpoint = Server.Endpoints.Session.Patch(session.Id);

                var request = new SessionPatchRequest(session);
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