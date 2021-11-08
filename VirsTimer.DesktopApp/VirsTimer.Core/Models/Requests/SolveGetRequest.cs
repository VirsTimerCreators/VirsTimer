namespace VirsTimer.Core.Models.Requests
{
    internal class SolveGetRequest
    {
        public string Id { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string SessionId { get; init; } = string.Empty;
        public string Scramble { get; init; } = string.Empty;
        public long Time { get; init; } = 0;
        public long Timestamp { get; init; } = 0;
        public string Solved { get; init; } = string.Empty;

        public Solve ToSolve(Session session)
        {
            return new Solve(session, Id!, Time, SolveServerFlags.ToSolveFlag(Solved), new System.DateTime(Timestamp), Scramble);
        }
    }
}
