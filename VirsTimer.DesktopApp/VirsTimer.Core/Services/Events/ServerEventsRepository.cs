using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Events
{
    public class ServerEventsRepository : AbstractServerRepository, IEventsRepository
    {
        public ServerEventsRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        public async Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync()
        {
            try
            {
                using var client = CreateHttpClientWithAuth();
                var endpoint = CreateEndpoint(Server.Endpoints.Events, Server.Endpoints.Users, UserClient.Id);
                var events = await client.GetFromJsonAsync<IReadOnlyList<Event>>(endpoint).ConfigureAwait(false) ?? Array.Empty<Event>();

                return new RepositoryResponse<IReadOnlyList<Event>>(events);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IReadOnlyList<Event>>(ex.StatusCode!, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IReadOnlyList<Event>>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }
    }
}
