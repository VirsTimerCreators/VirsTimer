namespace VirsTimer.Core.Models.Requests
{
    class SessionPatchRequest
    {
        public string Name { get; init; } = string.Empty;

        public SessionPatchRequest(Session session)
        {
            Name = session.Name;
        }
    }
}
