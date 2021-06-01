using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Scrambles
{
    /// <summary>
    /// <see cref="IScrambleGenerator"/> implementation for getting scrambles from application server side.
    /// </summary>
    public class ServerScrambleGenerator : IScrambleGenerator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerScrambleGenerator"/> class.
        /// </summary>
        public ServerScrambleGenerator(IHttpClientFactory httpClientFactory)
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
            using var httpClient = _httpClientFactory.CreateClient(Server.ScrambleEndpoint);
            var tasks = Enumerable.Range(0, scramblesAmount).Select(i => httpClient.GetFromJsonAsync<Scramble>(@event.Name));
            var scrambles = await Task.WhenAll(tasks).ConfigureAwait(false);
            return scrambles == null
                ? Array.Empty<Scramble>()
                : scrambles.Where(scramble => scramble != null).ToList();
        }
    }
}
