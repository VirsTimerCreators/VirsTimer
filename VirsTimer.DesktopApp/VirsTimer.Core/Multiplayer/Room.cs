using System.Collections.Generic;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Multiplayer
{
    public class Room
    {
        public string Id { get; }
        public string AccessCode { get; }
        public IReadOnlyList<RoomScramble> Scrambles { get; }
        public RoomStatus Status { get; set; }
        public IList<RoomUser> Users { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room(string id, string accesCode, IEnumerable<RoomScramble> scrambles, IList<RoomUser> users)
        {
            Id = id;
            AccessCode = accesCode;
            Scrambles = new List<RoomScramble>(scrambles);
            Status = RoomStatus.Open;
            Users = users;
        }
    }
}