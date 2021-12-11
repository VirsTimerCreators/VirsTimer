using System.Collections.Generic;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Multiplayer
{
    public class Room
    {
        public string Id { get; }
        public string AccessCode { get; }
        public IReadOnlyList<Scramble> Scrambles { get; }
        public RoomStatus Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room(string id, string accesCode, IEnumerable<Scramble> scrambles)
        {
            Id = id;
            AccessCode = accesCode;
            Scrambles = new List<Scramble>(scrambles);
            Status = RoomStatus.Open;
        }
    }
}