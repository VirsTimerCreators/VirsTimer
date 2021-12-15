using System.Collections.Generic;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomNotification
    {
        public RoomStatus Status { get; set; }
        public IReadOnlyList<RoomUser> RoomUsers { get; set; }
    }
}