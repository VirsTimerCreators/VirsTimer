using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public interface IEventsGetter
    {
        Task<IReadOnlyList<Event>> GetEventsAsync();
    }
}
