using System.Collections.Generic;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Scrambles
{
    /// <summary>
    /// Gathers <see cref="ICustomScrambleGenerator"/>.
    /// </summary>
    public interface ICustomScrambleGeneratorsCollector
    {
        /// <summary>
        /// Get all <see cref="ICustomScrambleGenerator"/> implementations.
        /// </summary>
        public IReadOnlyList<ICustomScrambleGenerator> GetCustomScrambleGenerators();
    }
}