using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Rooms
{
    public interface IRoomsService
    {
        public Task<RoomResponse> CreateRoomAsync(string eventName);
        public Task<RoomResponse> JoinRoomAsync(string accessCode);
        public Task SendScrambleAsync(Scramble scramble);
        public IObservable<RoomNotification> Notifications { get; } 
    }

    public class RoomResponse
    {
        public string AccessCode { get; set; }
        public IReadOnlyList<Scramble> Scrambles { get; set; }
    }
}
