using System.Collections.Generic;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomNotification
    {
        public IReadOnlyList<RoomUser> RoomUsers { get; set; }
    }
}