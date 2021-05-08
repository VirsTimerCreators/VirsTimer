using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public interface IPastSolvesGetter
    {
        Task<IReadOnlyList<Solve>> GetSolvesAsync(Event @event, Session session);
    }
}
