namespace VirsTimer.Core.Models.Requests
{
    internal class SolvePostRequest
    {
        public string SessionId { get; init; } = string.Empty;
        public string Scramble { get; init; } = string.Empty;
        public long Time { get; init; } = 0;
        public long Timestamp { get; init; } = 0;
        public string Solved { get; init; } = string.Empty;

        public SolvePostRequest(Solve solve)
        {
            SessionId = solve.Session.Id!;
            Scramble = solve.Scramble;
            Time = solve.Time;
            Timestamp = solve.Date.Ticks;
            Solved = SolveServerFlags.ConvertFromSolveFlag(solve.Flag);
        }
    }
}