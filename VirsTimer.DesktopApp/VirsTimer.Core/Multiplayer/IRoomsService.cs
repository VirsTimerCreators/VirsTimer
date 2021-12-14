using System;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Multiplayer
{
    public interface IRoomsService
    {
        public Task<RepositoryResponse<Room>> CreateRoomAsync(string eventName, int solvesAmount);
        public Task<RepositoryResponse<Room>> JoinRoomAsync(string accessCode);
        public Task<RepositoryResponse> StartRoomAsync(string roomId);
        public Task<RepositoryResponse> CloseRoomAsync(string roomId);
        public Task<RepositoryResponse> SendSolveAsync(RoomSolve solve);
        public IObservable<RoomNotification> Notifications { get; }
    }
}