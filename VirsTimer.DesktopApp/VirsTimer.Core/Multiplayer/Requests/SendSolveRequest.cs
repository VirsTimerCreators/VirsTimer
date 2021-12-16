using VirsTimer.Core.Models.Requests;

namespace VirsTimer.Core.Multiplayer.Requests
{
    internal class SendSolveRequest
    {
        public string ScrambleId { get; set; }
        public long Time { get; set; }
        public long Timestamp { get; set; }
        public string Solved { get; set; }

        public SendSolveRequest(RoomSolve solve)
        {
            ScrambleId = solve.ScrambleId;
            Time = solve.Time;
            Timestamp = solve.Date.Ticks;
            Solved = SolveServerFlags.ConvertFromSolveFlag(solve.Flag);
        }
    }
}