using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Export
{
    /// <summary>
    /// Exports solves from file.
    /// </summary>
    public interface ISolvesFileExporter
    {
        /// <summary>
        /// Exports solves.
        /// </summary>
        /// <param name="scrambles"></param>
        /// <returns></returns>
        public Task<string> ExportAsync(IReadOnlyList<Solve> scrambles);
    }
}