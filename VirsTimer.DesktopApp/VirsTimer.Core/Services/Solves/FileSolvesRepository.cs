using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Solves
{
    /// <summary>
    /// <see cref="ISolvesRepository"/> implementation that manages solves in local file.
    /// </summary>
    public class FileSolvesRepository : ISolvesRepository
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSolvesRepository"/> class.
        /// </summary>
        /// <param name="fileSystem">File system.</param>
        public FileSolvesRepository(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Gets solves from local file by given <paramref name="event"/> and <paramref name="session"/>. 
        /// </summary>
        public async Task<IReadOnlyList<Solve>> GetSolvesAsync(Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            if (!_fileSystem.File.Exists(targetFile))
                return Array.Empty<Solve>();

            using var stream = _fileSystem.File.OpenRead(targetFile);
            var solves = await JsonSerializer.DeserializeAsync<IReadOnlyList<Solve>>(stream).ConfigureAwait(false) ?? Array.Empty<Solve>();
            await Task.WhenAll(
                solves.Select(solve => Task.Run(() =>
                {
                    solve.Event = @event;
                    solve.Session = session;
                })))
                .ConfigureAwait(false);

            return solves;
        }

        /// <summary>
        /// Saves solve in local file.
        /// </summary>
        public async Task SaveSolveAsync(Solve solve)
        {
            var (solves, stream) = await LoadSolvesAsync(solve.Event, solve.Session).ConfigureAwait(false);
            solve.Id ??= Guid.NewGuid().ToString();
            solves.Add(solve);
            using (stream)
                await JsonSerializer.SerializeAsync(stream, solves).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates solve in local file.
        /// </summary>
        public async Task UpdateSolveAsync(Solve solve)
        {
            var (solves, stream) = await LoadSolvesAsync(solve.Event, solve.Session).ConfigureAwait(false);
            var foundSolve = solves.Find(x => x.Id == solve.Id);
            if (foundSolve == null)
                return;

            foundSolve.Flag = solve.Flag;
            using (stream)
                await JsonSerializer.SerializeAsync(stream, solves).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes solve from local file.
        /// </summary>
        public async Task DeleteSolveAsync(Solve solve)
        {
            var (solves, stream) = await LoadSolvesAsync(solve.Event, solve.Session).ConfigureAwait(false);
            var foundSolve = solves.Find(x => x.Id == solve.Id);
            if (foundSolve == null)
                return;

            solves.Remove(foundSolve);
            using (stream)
                await JsonSerializer.SerializeAsync(stream, solves).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads solves from file to List.
        /// </summary>
        /// <returns>List of solves and file reseted Stream.</returns>
        private async Task<(List<Solve>, Stream)> LoadSolvesAsync(Event @event, Session session)
        {
            var targetFile = _fileSystem.Path.Combine(Application.ApplicationDataDirectoryPath, @event.Name, $"{session.Name}{FileExtensions.Json}");
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(targetFile));

            var stream = _fileSystem.File.Open(targetFile, FileMode.OpenOrCreate);
            var solves = await JsonSerializer.DeserializeAsync<List<Solve>>(stream).ConfigureAwait(false) ?? new List<Solve>();
            stream.SetLength(0);

            return (solves, stream);
        }
    }
}
