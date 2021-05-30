using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Sessions
{
    public interface ISessionsManager
    {
        Task<Session> AddSessionAsync(Event @event, string name);
        Task<IReadOnlyList<Session>> GetAllSessionsAsync(Event @event);
        Task<Session> RenameSessionAsync(Event @event, Session session, string newName);
    }
}
