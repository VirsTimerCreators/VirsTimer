using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Naming;

namespace VirsTimer.Core.Export
{
    /// <summary>
    /// <see cref="ISolvesCsvExporter"/> standard implementation.
    /// </summary>
    public class SolvesCsvExporter : ISolvesCsvExporter
    {
        private static readonly NameGenerator NameGenerator = new("VirsTimerScramblesExport");

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<IReadOnlyList<Solve>> ImportAsync(string path)
        {
            var lines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);
            var soles = lines.Select(line => CsvLineToSolve(line)).ToList();
            return soles;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<string> ExportAsync(IReadOnlyList<Solve> solves)
        {

            if (Directory.Exists(Application.ApplicationDocumentsPath) is false)
                Directory.CreateDirectory(Application.ApplicationDocumentsPath);

            var taken = Directory.EnumerateFiles(Application.ApplicationDocumentsPath).Select(file => Path.GetFileNameWithoutExtension(file));
            var nextName = NameGenerator.GenerateNext(taken);
            var destination = Path.Combine(Application.ApplicationDocumentsPath, $"{nextName}.csv");

            var lines = solves.Select(solve => SolveToCsvLine(solve));
            await File.WriteAllLinesAsync(destination, lines).ConfigureAwait(false);

            return destination;
        }

        private static string SolveToCsvLine(Solve solve)
        {
            var sb = new StringBuilder();
            sb.Append(solve.Id);
            sb.Append(";");
            sb.Append(solve.Scramble);
            sb.Append(";");
            sb.Append(solve.Time);
            sb.Append(";");
            sb.Append(solve.Flag);
            sb.Append(";");
            sb.Append(solve.Date);
            sb.Append(";");

            return sb.ToString();
        }

        private static Solve CsvLineToSolve(string line)
        {
            var values = line.Split(";");
            var id = values[0];
            var scramble = values[1];
            var time = long.Parse(values[2]);
            var flag = Enum.Parse<SolveFlag>(values[3]);
            var date = DateTime.Parse(values[4]);

            return new Solve(id, time, flag, date, scramble);
        }
    }
}