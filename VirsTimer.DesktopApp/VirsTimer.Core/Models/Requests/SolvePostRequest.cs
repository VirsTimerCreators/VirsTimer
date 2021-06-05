namespace VirsTimer.Core.Models.Requests
{
    internal class SolvePostRequest
    {
        public string? Id { get; init; }
        public string UserId { get; init; } = string.Empty;
        public string SessionId { get; init; } = string.Empty;
        public string Scramble { get; init; } = string.Empty;
        public long Time { get; init; } = 0;
        public long TimeStamp { get; init; } = 0;
        public string Solved { get; init; } = string.Empty;

        public SolvePostRequest(string userId, Solve solve)
        {
            UserId = userId;
            Id = solve.Id;
            SessionId = solve.Session.Id!;
            Scramble = solve.Scramble;
            Time = solve.Time;
            TimeStamp = solve.Date.Ticks;
            Solved = SolveServerFlags.ConvertFromSolveFlag(solve.Flag);
        }
    }
}
