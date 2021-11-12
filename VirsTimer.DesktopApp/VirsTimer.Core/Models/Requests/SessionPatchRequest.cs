namespace VirsTimer.Core.Models.Requests
{
    internal class SessionPatchRequest
    {
        public string Name { get; init; }

        public SessionPatchRequest(Session session)
        {
            Name = session.Name;
        }
    }
}
