using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public interface ISessionsManager
    {
        Task AddSessionAsync(string @event, Session session);
        Task<IReadOnlyList<Session>> GetSessionsAsync(string @event);
    }
}
