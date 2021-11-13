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
            if (Constants.Events.All.All(e => e != @event.Name))
                return Enumerable.Repeat(Scramble.Empty, scramblesAmount).ToList();

            using var httpClient = _httpClientFactory.CreateClient();
            var endpoint = Server.Endpoints.Scramble.Get(GetServerEventName(@event.Name));
            var tasks = Enumerable.Range(0, scramblesAmount).Select(i => httpClient.GetFromJsonAsync<Scramble>(endpoint));
            var scrambles = await Task.WhenAll(tasks).ConfigureAwait(false);
            if (scrambles is null)
                return Array.Empty<Scramble>();
            return scrambles.Where(scramble => scramble != null).ToList()!;
        }

        private static string GetServerEventName(string eventName)
        {
            return eventName switch
            {
                Constants.Events.TwoByTwo => Server.Events.TwoByTwo,
                Constants.Events.ThreeByThree => Server.Events.ThreeByThree,
                Constants.Events.FourByFour => Server.Events.FourByFour,
                Constants.Events.FiveByFive => Server.Events.FiveByFive,
                Constants.Events.SixBySix => Server.Events.SixBySix,
                Constants.Events.SevenBySeven => Server.Events.SevenBySeven,
                Constants.Events.Pyraminx => Server.Events.Pyraminx,
                Constants.Events.Megaminx => Server.Events.Megaminx,
                Constants.Events.Skewb => Server.Events.Skewb,
                Constants.Events.SquareOne => Server.Events.SquareOne,
                Constants.Events.Clock => Server.Events.Clock,
                Constants.Events.ThreeByThreeBlindfold => Server.Events.ThreeByThreeBlindfold,
                Constants.Events.ThreeByThreeOneHand => Server.Events.ThreeByThreeOneHand,
                Constants.Events.FourByFourBlindfold => Server.Events.FourByFourBlindfold,
                Constants.Events.FiveByFiveBlindfold => Server.Events.FiveByFiveBlindfold,
                _ => throw new ArgumentException(nameof(eventName))
            };
        }
    }
}