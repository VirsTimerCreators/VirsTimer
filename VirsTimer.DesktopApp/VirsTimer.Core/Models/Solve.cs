using System;
using System.Text.Json.Serialization;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Model representing Solve.
    /// </summary>
    public class Solve
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
        /// Solve time.
        /// </summary>
        public long Time { get; }

        /// <summary>
        /// Solve time as TimeSpan.
        /// </summary>
        [JsonIgnore]
        public TimeSpan TimeAsSpan => TimeSpan.FromTicks(Time);

        /// <summary>
        /// Solve flag.
        /// </summary>
        public SolveFlag Flag { get; set; }

        /// <summary>
        /// Solve creation date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Solve scramble.
        /// </summary>
        public string Scramble { get; }

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
