using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services.Solves
{
    public partial class SolvesFileRepository
    {
        private class Cache
        {
            private readonly IFileSystem _fileSystem;
            private string _savedEventId;

            public string SavedSessionId { get; private set; }

            public List<Solve> LoadedSolves { get; private set; }

            public Cache(string sessionId, IFileSystem fileSystem)
            {
                _fileSystem = fileSystem;
                _savedEventId = string.Empty;
                SavedSessionId = sessionId;
                LoadedSolves = new List<Solve>();
            }

            public async Task RefreshCacheAsync(Session session)
            {
                if (session.Id is null)
                    throw new ArgumentNullException(nameof(session), "Session Id cannot be null.");

                if (session.Event.Id is null)
                    throw new ArgumentNullException(nameof(session), "Session event Id cannot be null.");

                if (SavedSessionId == session.Id)
                    return;

                var targetFile = GetSessionFile(session.Event.Id, session.Id);
                using var stream = _fileSystem.File.OpenRead(targetFile);

                var solves = await JsonSerializer.DeserializeAsync<List<Solve>>(stream).ConfigureAwait(false) ?? new List<Solve>();
                solves.ForEach(solve => solve.Session = session);

                _savedEventId = session.Event.Id;
                SavedSessionId = session.Id;
                LoadedSolves = solves;
            }

            public async Task UpdateCacheTargetAsync()
            {
                var targetFile = GetSessionFile(_savedEventId, SavedSessionId);
                using var stream = _fileSystem.File.OpenWrite(targetFile);
                stream.SetLength(0);
                await JsonSerializer.SerializeAsync(stream, LoadedSolves).ConfigureAwait(false);
            }

            private string GetSessionFile(string eventId, string sessionId)
            {
                return _fileSystem.Path.Combine(Application.ApplicationDirectoryPath, eventId, $"{sessionId}{FileExtensions.Json}");
            }
        }
    }
}