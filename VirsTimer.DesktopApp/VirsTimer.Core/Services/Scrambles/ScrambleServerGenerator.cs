using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Scrambles
{
    /// <summary>
    /// <see cref="IScrambleGenerator"/> implementation for getting scrambles from application server side.
    /// </summary>
    public class ScrambleServerGenerator : IScrambleGenerator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrambleServerGenerator"/> class.
        /// </summary>
        public ScrambleServerGenerator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get scramlbes from server.
        /// </summary>
        /// <param name="event">Event of which type will be scrambles.</param>
        /// <param name="scramblesAmount">Amount of scrambles.</param>
        public async Task<IReadOnlyList<Scramble>> GenerateScrambles(Event @event, int scramblesAmount)
        {
            if (Constants.Events.Predefined.All(e => e != @event.Name))
                return Enumerable.Repeat(new Scramble(), scramblesAmount).ToList();

            using var httpClient = _httpClientFactory.CreateClient();
            var serverEventName = Server.Events.GetServerEventName(@event.Name);
            var endpoint = Server.Endpoints.Scramble.Get(serverEventName);
            var tasks = Enumerable.Range(0, scramblesAmount).Select(i => httpClient.GetFromJsonAsync<Scramble>(endpoint));
            var scrambles = await Task.WhenAll(tasks).ConfigureAwait(false);
            if (scrambles is null)
                return Array.Empty<Scramble>();
            return scrambles.Where(scramble => scramble != null).ToList()!;
        }
    }
}