using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Events
{
    /// <summary>
    /// Virs timer server api <see cref="IEventsRepository"/> implementation. 
    /// </summary>
    public class ServerEventsRepository : IEventsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEventsRepository"/> class.
        /// </summary>
        public ServerEventsRepository(
            IHttpClientFactory httpClientFactory,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync()
        {
            using var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var httpRequestFunc = () => client.GetAsync(Server.Endpoints.Event.Get);
            var response = await _httpResponseHandler.HandleAsync<IReadOnlyList<Event>>(httpRequestFunc).ConfigureAwait(false);
            return response;
        }

        // dodać poniżesze funkcje gdy backend będzie gotowy
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<Event>> AddEventAsync(Event @event)
        {
            @event.Id = Guid.NewGuid().ToString();
            return new RepositoryResponse<Event>(@event);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteEventAsync(Event @event)
        {
            return RepositoryResponse.Ok;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateEventAsync(Event @event)
        {
            return RepositoryResponse.Ok;
        }
    }
}