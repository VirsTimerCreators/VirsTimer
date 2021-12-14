using System;
using System.Collections.Generic;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomUser
    {
        public string Name { get; init; } = string.Empty;
        public IReadOnlyList<RoomSolve> Solves { get; init; } = Array.Empty<RoomSolve>();
    }
}
