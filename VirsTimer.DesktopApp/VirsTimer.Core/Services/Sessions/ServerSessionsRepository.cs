using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

namespace VirsTimer.Core.Services.Sessions
{
    class ServerSessionsRepository : AbstractServerRepository, ISessionRepository
    {
        public ServerSessionsRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        public async Task<RepositoryResponse> AddSessionAsync(Session session)
        {
            try
            {
                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Events, session.Event.Id, Server.Endpoints.Sessions);
                var httpResponse = await client.PostAsJsonAsync(endpoint, session).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync(httpResponse).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse> DeleteSessionAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    throw new ArgumentException("Session Id cannot be null.", nameof(session));

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Events, session.Event.Id, Server.Endpoints.Sessions, session.Id);
                var httpResponse = await client.DeleteAsync(endpoint).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync(httpResponse).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse<IReadOnlyList<Session>>> GetSessionsAsync(Event @event)
        {
            try
            {
                if (@event.Id == null)
                    throw new ArgumentException("Session Id cannot be null.", nameof(@event));

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Events, @event.Id);
                var sessions = await client.GetFromJsonAsync<IReadOnlyList<Session>>(endpoint).ConfigureAwait(false) ?? Array.Empty<Session>();
                foreach (var session in sessions)
                    session.Event = @event;

                return new RepositoryResponse<IReadOnlyList<Session>>(sessions);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse> UpdateSessionAsync(Session session)
        {
            try
            {
                if (session.Id == null)
                    throw new ArgumentException("Session Id cannot be null.", nameof(session));

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Events, session.Event.Id, Server.Endpoints.Sessions, session.Id);
                var sessionSerialized = JsonSerializer.Serialize(session);
                using var content = new StringContent(sessionSerialized);
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
