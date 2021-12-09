using System;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Rooms
{
    public class RoomServerService : IRoomsService
    {
        public IObservable<RoomNotification> Notifications => throw new NotImplementedException();

        public async Task<RoomResponse> CreateRoomAsync(string eventName)
        {
            return new RoomResponse();
        }

        public async Task<RoomResponse> JoinRoomAsync(string accessCode)
        {
            return new RoomResponse();
        }

        public async Task SendScrambleAsync(Scramble scramble)
        {
            throw new NotImplementedException();
        }
    }
}
