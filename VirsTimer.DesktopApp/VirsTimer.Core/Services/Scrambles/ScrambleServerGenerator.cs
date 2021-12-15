using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Scrambles
{
    /// <summary>
    /// <see cref="IScrambleGenerator"/> implementation for getting scrambles from application server side.
    /// </summary>
    public class ScrambleServerGenerator : IScrambleGenerator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrambleServerGenerator"/> class.
        /// </summary>
        public ScrambleServerGenerator(
            IHttpClientFactory httpClientFactory,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
        }

        /// <summary>
        /// Get scramlbes from server.
        /// </summary>
        /// <param name="event">Event of which type will be scrambles.</param>
        /// <param name="scramblesAmount">Amount of scrambles.</param>
        public async Task<RepositoryResponse<IReadOnlyList<Scramble>>> GenerateScrambles(Event @event, int scramblesAmount)
        {
            if (Constants.Events.Predefined.All(e => e != @event.Name))
            {
                var empty = Enumerable.Repeat(new Scramble(), scramblesAmount).ToList();
                return new RepositoryResponse<IReadOnlyList<Scramble>>(empty!);
            }

            using var httpClient = _httpClientFactory.CreateClient();
            var serverEventName = Server.Events.GetServerEventName(@event.Name);
            var endpoint = Server.Endpoints.Scramble.Get(serverEventName, scramblesAmount);
            var httpRequestFunc = () => httpClient.GetAsync(endpoint);
            var response = await _httpResponseHandler.HandleAsync<IReadOnlyList<Scramble>>(httpRequestFunc).ConfigureAwait(false);
            return response;
        }
    }
}