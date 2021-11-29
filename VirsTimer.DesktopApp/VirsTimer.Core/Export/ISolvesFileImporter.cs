using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Export
{
    /// <summary>
    /// Imports solves into file.
    /// </summary>
    public interface ISolvesFileImporter
    {
        /// <summary>
        /// Imports solves.
        /// </summary>
        public Task<IReadOnlyList<Solve>> ImportAsync(string path);
    }
}