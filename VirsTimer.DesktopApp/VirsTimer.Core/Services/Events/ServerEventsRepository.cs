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
    /// <summary>
    /// Virs timer server api <see cref="IEventsRepository"/> implementation. 
    /// </summary>
    public class ServerEventsRepository : AbstractServerRepository, IEventsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEventsRepository"/> class.
        /// </summary>
        public ServerEventsRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
            : base(httpClientFactory, userClient)
        { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IReadOnlyList<Event>>> GetEventsAsync()
        {
            try
            {
                using var client = CreateHttpClientWithAuth();
                var events = await client.GetFromJsonAsync<IReadOnlyList<Event>>(Server.Endpoints.Event.Get).ConfigureAwait(false) ?? Array.Empty<Event>();

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