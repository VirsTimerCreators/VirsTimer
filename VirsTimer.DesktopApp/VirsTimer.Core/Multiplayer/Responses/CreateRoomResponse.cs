using System;

namespace VirsTimer.Core.Multiplayer.Responses
{
    internal class CreateRoomResponse
    {
        public string Id { get; init; } = string.Empty;
        public string JoinCode { get; init; } = string.Empty;
        public RoomScramble[] Scrambles { get; init; } = Array.Empty<RoomScramble>();
        public string[] Users { get; init; } = Array.Empty<string>();
    }
}