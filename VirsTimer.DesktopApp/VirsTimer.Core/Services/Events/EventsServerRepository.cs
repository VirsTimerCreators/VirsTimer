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
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Events
{
    /// <summary>
    /// Virs timer server api <see cref="IEventsRepository"/> implementation. 
    /// </summary>
    public class EventsServerRepository : IEventsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsServerRepository"/> class.
        /// </summary>
        public EventsServerRepository(
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
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var httpRequestFunc = () => client.GetAsync(Server.Endpoints.Event.Get);
            var response = await _httpResponseHandler.HandleAsync<IReadOnlyList<Event>>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
            {
                response!.Value.ForEach(e =>
                {
                    if (Server.Events.All.Contains(e.Name))
                        e.Name = Server.Events.GetEventName(e.Name);
                });
            };

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> AddEventAsync(Event @event)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new EventPostRequest(@event.Name);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Event.Post, request, Json.ServerSerializerOptions);
            var response = await _httpResponseHandler.HandleAsync<EventPostResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
                @event.Id = response.Value!.Id;

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> UpdateEventAsync(Event @event)
        {
            if (@event.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Event.Patch(@event.Id);
            var request = new EventPatchRequest(@event.Name);
            var httpResponseFunc = () => client.PatchAsJsonAsync(endpoint, request);
            var response = await _httpResponseHandler.HandleAsync(httpResponseFunc).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> DeleteEventAsync(Event @event)
        {
            if (@event.Id == null)
                return new RepositoryResponse(RepositoryResponseStatus.ClientError, "Event Id cannot be null.");

            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var endpoint = Server.Endpoints.Event.Delete(@event.Id);
            var httpResponseFunc = () => client.DeleteAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync(httpResponseFunc).ConfigureAwait(false);

            return response;
        }
    }
}