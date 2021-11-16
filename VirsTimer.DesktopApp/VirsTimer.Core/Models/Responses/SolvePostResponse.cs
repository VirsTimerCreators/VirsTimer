namespace VirsTimer.Core.Models.Responses
{
    internal class SolvePostResponse
    {
        public string Id { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string SessionId { get; init; } = string.Empty;
        public string Scramble { get; init; } = string.Empty;
        public long Time { get; init; } = 0;
        public long Timestamp { get; init; } = 0;
        public string Solved { get; init; } = string.Empty;
    }
}