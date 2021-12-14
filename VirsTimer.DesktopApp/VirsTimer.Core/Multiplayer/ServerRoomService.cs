﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Multiplayer.Requests;
using VirsTimer.Core.Multiplayer.Responses;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomServerService : IRoomsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        public RoomServerService(
            IHttpClientFactory httpClientFactory,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
        }

        public IObservable<RoomNotification> Notifications => throw new NotImplementedException();

        public async Task<RepositoryResponse<Room>> CreateRoomAsync(string eventName, int solvesAmount)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new CreateRoomRequest(solvesAmount, eventName);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Room.Post, request);
            var response = await _httpResponseHandler.HandleAsync<CreateRoomResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
            {
                var room = new Room(response.Value!.Id, response.Value.JoinCode, Array.Empty<Scramble>());
                return new RepositoryResponse<Room>(response, room);
            }

            return new RepositoryResponse<Room>(response, null!);
        }

        public async Task<RepositoryResponse<Room>> JoinRoomAsync(string accessCode)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new JoinRoomRequest { JoinCode = accessCode };
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Room.Join, request);
            var response = await _httpResponseHandler.HandleAsync<JoinRoomResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
            {
                var room = new Room(response.Value!.Id, accessCode, Array.Empty<Scramble>());
                return new RepositoryResponse<Room>(response, room);
            }

            return new RepositoryResponse<Room>(response, null!);
        }

        public Task<RepositoryResponse> SendScrambleAsync(Scramble scramble)
        {
            return Task.FromResult(RepositoryResponse.Ok);
        }
    }
}