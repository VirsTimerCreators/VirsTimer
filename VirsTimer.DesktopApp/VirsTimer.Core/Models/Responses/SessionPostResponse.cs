namespace VirsTimer.Core.Models.Responses
{
    internal class SessionPostResponse
    {
        public string Id { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string EventId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}