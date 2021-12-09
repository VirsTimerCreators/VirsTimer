using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Models.Responses
{
    public class RoomNotification
    {
        public IReadOnlyList<Scramble> Scrambles { get; set; }
        public IReadOnlyList<RoomUser> RoomUsers { get; set; }
    }
    public class RoomUser
    {
        public string Name { get; set; }
        public IReadOnlyList<Solve> Solves { get; set; }
    }
}
