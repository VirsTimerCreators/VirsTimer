using System.Collections.Generic;

namespace VirsTimer.Core.Multiplayer.Responses
{
    internal class RoomNotificationResponse
    {
        public Dictionary<string, List<RoomNotificationSolve>> Users { get; init; }
    }

    internal class RoomNotificationSolve
    {
        public string Id { get; init; }
        public long Time { get; init; }
        public string Solved { get; init; }
        public long Timestamp { get; init; }
    }
}