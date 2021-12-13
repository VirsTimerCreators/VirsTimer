using System;

namespace VirsTimer.Core.Multiplayer.Responses
{
    internal class JoinRoomResponse
    {
        public string Id { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
        public RoomScramble[] ScrambleIds { get; set; } = Array.Empty<RoomScramble>();
        public string[] Users { get; set; } = Array.Empty<string>();
    }
}