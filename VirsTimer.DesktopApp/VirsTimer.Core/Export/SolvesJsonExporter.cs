using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Naming;

namespace VirsTimer.Core.Export
{
    /// <summary>
    /// <see cref="ISolvesJsonExporter"/> standard implementation.
    /// </summary>
    public class SolvesJsonExporter : ISolvesJsonExporter
    {
        private static readonly NameGenerator NameGenerator = new("VirsTimerScramblesExport");

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<IReadOnlyList<Solve>> ImportAsync(string path)
        {
            using var stream = File.OpenRead(path);
            var scrambles = await JsonSerializer.DeserializeAsync<IReadOnlyList<Solve>>(stream).ConfigureAwait(false);
            return scrambles ?? Array.Empty<Solve>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<string> ExportAsync(IReadOnlyList<Solve> scrambles)
        {
            if (Directory.Exists(Application.ApplicationDocumentsPath) is false)
                Directory.CreateDirectory(Application.ApplicationDocumentsPath);

            var taken = Directory.EnumerateFiles(Application.ApplicationDocumentsPath).Select(file => Path.GetFileNameWithoutExtension(file));
            var nextName = NameGenerator.GenerateNext(taken);
            var destination = Path.Combine(Application.ApplicationDocumentsPath, $"{nextName}.json");

            using var stream = File.OpenWrite(destination);
            await JsonSerializer.SerializeAsync(stream, scrambles).ConfigureAwait(false);

            return nextName;
        }
    }
}