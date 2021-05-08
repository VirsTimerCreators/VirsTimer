using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public interface IScrambleGenerator
    {
        Task<IReadOnlyList<Scramble>> GenerateScrambles(Event @event, int scramblesAmount);
    }
}
