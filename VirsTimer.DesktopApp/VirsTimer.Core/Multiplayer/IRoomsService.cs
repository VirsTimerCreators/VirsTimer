using System;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Multiplayer
{
    public interface IRoomsService
    {
        public Task<RepositoryResponse<Room>> CreateRoomAsync(string eventName, int solvesAmount);
        public Task<RepositoryResponse<Room>> JoinRoomAsync(string accessCode);
        public Task<RepositoryResponse> SendSolveAsync(string scrambleId, Solve solve);
        public IObservable<RoomNotification> Notifications { get; }
    }
}