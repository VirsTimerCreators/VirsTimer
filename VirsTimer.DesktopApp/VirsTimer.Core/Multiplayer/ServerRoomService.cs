using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Multiplayer.Requests;
using VirsTimer.Core.Multiplayer.Responses;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomServerService : IRoomsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;
        private readonly Subject<RoomNotification> _subject;

        private volatile string lastNotificationData = string.Empty;

        public RoomServerService(
            IHttpClientFactory httpClientFactory,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
            _subject = new Subject<RoomNotification>();
        }

        public IObservable<RoomNotification> Notifications => _subject.AsObservable();

        public async Task<RepositoryResponse<Room>> CreateRoomAsync(string eventName, int solvesAmount)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new CreateRoomRequest(solvesAmount, eventName);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Room.Post, request);
            var response = await _httpResponseHandler.HandleAsync<CreateRoomResponse>(httpRequestFunc).ConfigureAwait(false);
            if (response.IsSuccesfull)
            {
                var users = response.Value!.Users.Select(user => new RoomUser { Name = user }).ToList();
                var room = new Room(response.Value!.Id, response.Value.JoinCode, response.Value.Scrambles, users);
                AttachToNotifications(room.Id);
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
                var users = response.Value!.Users.Select(user => new RoomUser { Name = user }).ToList();
                var room = new Room(response.Value!.Id, accessCode, response.Value.Scrambles, users);
                return new RepositoryResponse<Room>(response, room);
            }

            return new RepositoryResponse<Room>(response, null!);
        }

        public async Task<RepositoryResponse> SendSolveAsync(RoomSolve solve)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var request = new SendSolveRequest(solve);
            var httpRequestFunc = () => client.PostAsJsonAsync(Server.Endpoints.Room.Join, request);
            var response = await _httpResponseHandler.HandleAsync(httpRequestFunc).ConfigureAwait(false);
            return response;
        }

        public async Task<RepositoryResponse> StartRoomAsync(string roomId)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var httpRequestFunc = () => client.PatchAsJsonAsync(Server.Endpoints.Room.Patch(roomId), ChangeRoomStatusRequest.Start);
            var response = await _httpResponseHandler.HandleAsync(httpRequestFunc).ConfigureAwait(false);
            return response;
        }

        public async Task<RepositoryResponse> CloseRoomAsync(string roomId)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var httpRequestFunc = () => client.PatchAsJsonAsync(Server.Endpoints.Room.Patch(roomId), ChangeRoomStatusRequest.Close);
            var response = await _httpResponseHandler.HandleAsync(httpRequestFunc).ConfigureAwait(false);
            return response;
        }

        private async void AttachToNotifications(string roomId)
        {
            var client = _httpClientFactory.CreateClient(HttpClientNames.UserAuthorized);
            var stream = await client.GetStreamAsync(Server.Endpoints.Room.Notifications(roomId));
            using var reader = new StreamReader(stream);
            while (reader.EndOfStream is false)
            {
                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is null || line.Length < 6)
                    continue;

                if (line[..5] != "data:")
                    continue;

                var cleared = line[5..];
                if (lastNotificationData == cleared)
                    continue;

                lastNotificationData = cleared;
                var roomNotificationResponse = JsonSerializer.Deserialize<RoomNotificationResponse>(lastNotificationData, Json.ServerSerializerOptions);
                if (roomNotificationResponse is null)
                    continue;

                var roomUsers = roomNotificationResponse.Users
                    .Select(solvesByUser =>
                    {
                        var solves = solvesByUser.Value.Select(x => new RoomSolve
                        {
                            Id = x.Id,
                            Time = x.Time,
                            Flag = SolveServerFlags.ToSolveFlag(x.Solved),
                            Date = new DateTime(x.Timestamp)
                        });

                        return new RoomUser
                        {
                            Name = solvesByUser.Key,
                            Solves = solves.OrderByDescending(x => x.Date).ToList()
                        };
                    })
                    .ToList();

                var notification = new RoomNotification
                {
                    RoomUsers = roomUsers
                };

                _subject.OnNext(notification);
            }
        }
    }
}