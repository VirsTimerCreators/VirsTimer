using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public interface ISolvesSaver
    {
        Task SaveSolvesAsync(IEnumerable<Solve> solves, string @event, string session);
    }
}
