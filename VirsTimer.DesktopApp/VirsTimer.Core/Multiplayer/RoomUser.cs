using System.Collections.Generic;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomUser
    {
        public string Name { get; }
        public IList<Scramble> Scrambles { get; }

        public RoomUser(string name, IList<Scramble> scrambles)
        {
            Name = name;
            Scrambles = scrambles;
        }

        public RoomUser(string name)
        {
            Name = name;
            Scrambles = new List<Scramble>();
        }
    }
}