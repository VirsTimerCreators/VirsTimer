namespace VirsTimer.Core.Models.Requests
{
    internal class SessionGetRequest
    {
        public string Id { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string EventId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        public Session ToSession(Event @event)
        {
            return new Session(@event, Id, Name);
        }
    }
}
