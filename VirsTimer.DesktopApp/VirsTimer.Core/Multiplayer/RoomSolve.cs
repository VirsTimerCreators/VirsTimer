using VirsTimer.Core.Models;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomSolve : SolveBase
    {
        /// <summary>
        /// Solve id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Scramble id.
        /// </summary>
        public string ScrambleId { get; init; } = string.Empty;
    }
}