using System;
using System.Text.Json.Serialization;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Model representing Solve.
    /// </summary>
    public class Solve : SolveBase
    {
        /// <summary>
        /// Solve session.
        /// </summary>
        [JsonIgnore]
        public Session Session { get; set; } = null!;

        /// <summary>
        /// Solve id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Solve scramble.
        /// </summary>
        public string Scramble { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solve"/> class.
        /// </summary>
        [JsonConstructor]
        public Solve(string id, long time, SolveFlag flag, DateTime date, string scramble)
        {
            Id = id;
            Time = time;
            Flag = flag;
            Date = date;
            Scramble = scramble;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solve"/> class.
        /// </summary>
        public Solve(Session session, string id, long time, SolveFlag flag, DateTime date, string scramble)
        {
            Session = session;
            Id = id;
            Time = time;
            Flag = flag;
            Date = date;
            Scramble = scramble;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Solve"/> class.
        /// </summary>
        public Solve(Session session, TimeSpan time, string scramble)
        {
            Session = session;
            Time = time.Ticks;
            Flag = SolveFlag.OK;
            Date = DateTime.Now;
            Scramble = scramble;
        }
    }
}
