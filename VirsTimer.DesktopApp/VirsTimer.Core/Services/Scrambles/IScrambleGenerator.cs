using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services
{
    /// <summary>
    /// Generates scrambles.
    /// </summary>
    public interface IScrambleGenerator
    {
        /// <summary>
        /// Generates scrambles for <paramref name="event"/> type cube.
        /// </summary>
        /// <param name="event">Event of which type will be scrambles.</param>
        /// <param name="scramblesAmount">Amount of scrambles.</param>
        Task<IReadOnlyList<Scramble>> GenerateScrambles(Event @event, int scramblesAmount);
    }
}