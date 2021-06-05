using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

namespace VirsTimer.Core.Services.Events
{
    public class ServerEventsRepository : AbstractServerRepository, IEventsRepository
    {
        public ServerEventsRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        public async Task<IReadOnlyList<Event>> GetEventsAsync()
        {
            using var client = CreateHttpClientWithAuth();
            var endpoint = CreateEndpoint(Server.Endpoints.Events);
            var events = await client.GetFromJsonAsync<IReadOnlyList<Event>>(endpoint).ConfigureAwait(false);
            return events ?? Array.Empty<Event>();
        }
    }
}
