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

namespace VirsTimer.Core.Services.Sessions
{
    /// <summary>
    /// Virs timer server api <see cref="SessionsServerRepository"/> implementation. 
    /// </summary>
    public class SessionsServerRepository : ISessionsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserClient _userClient;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionsServerRepository"/> class.
        /// </summary>
        public SessionsServerRepository(
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
        public async Task<RepositoryResponse> AddSessionAsync(Session session)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);

            var request = new SessionPostRequest(_userClient.Id, session);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Session.Post, request, Json.ServerSerializerOptions);
            var response = await _httpResponseHandler.HandleAsync<SessionPostResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
                session.Id = response.Value.Id;

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteSessionAsync(Session session)
        {
            if (session.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Session.Delete(session.Id);
            var httpResponseFunc = () => client.DeleteAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync(httpResponseFunc).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Session>>> GetSessionsAsync(Event @event)
        {
            if (@event.Id == null)
                return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Session.GetByEvent(@event.Id);
            var httpResponseFunc = () => client.GetAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync<SessionGetRequest[]>(httpResponseFunc).ConfigureAwait(false);
            if (!response.IsSuccesfull)
                return new RepositoryResponse<IReadOnlyList<Session>>(response, Array.Empty<Session>());

            if (response.Value.IsNullOrEmpty())
            {
                var session = new Session(@event, "Sesja1");
                var addSessionResponse = await AddSessionAsync(session).ConfigureAwait(false);
                return new RepositoryResponse<IReadOnlyList<Session>>(addSessionResponse, new[] { session });
            }

            var sessions = response.Value.Select(x => x.ToSession(@event)).ToList();

            return new RepositoryResponse<IReadOnlyList<Session>>(response, sessions);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateSessionAsync(Session session)
        {
            if (session.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Session.Patch(session.Id);
            var request = new SessionPatchRequest(session);
            var httpResponseFunc = () => client.PatchAsJsonAsync(endpoint, request);
            var response = await _httpResponseHandler.HandleAsync(httpResponseFunc).ConfigureAwait(false);

            return response;
        }
    }
}