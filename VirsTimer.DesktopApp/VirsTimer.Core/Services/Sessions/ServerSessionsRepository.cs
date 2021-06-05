﻿using System;
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
                var endpoint = CreateEndpoint(Server.Endpoints.Events);

                var content = new SessionPostRequest(UserClient.Id, session);
                var httpResponse = await client.PostAsJsonAsync(endpoint, content).ConfigureAwait(false);
                var response = await CreateRepositoryResponseAsync<SessionPostRequest>(httpResponse).ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                    session.Id = response.Value.Id;

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
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Sessions, session.Id);
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
                    return new RepositoryResponse<IReadOnlyList<Session>>(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Sessions, Server.Endpoints.Events, @event.Id, Server.Endpoints.Users, UserClient.Id);
                var sessionsResponse = await client.GetFromJsonAsync<SessionGetRequest[]>(endpoint).ConfigureAwait(false) ?? Array.Empty<SessionGetRequest>();
                var sessions = sessionsResponse.Select(x => x.ToSession(@event)).ToList();

                return new RepositoryResponse<IReadOnlyList<Session>>(sessions);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IReadOnlyList<Session>>((HttpStatusCode)ex.StatusCode!, ex.Message);
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
                    return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Session Id cannot be null.");

                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Sessions, session.Id);

                using var content = CreateJsonRequest(new SessionPatchRequest(session));
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