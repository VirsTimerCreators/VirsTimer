using System;
using System.Text.Json.Serialization;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    public class SolveBase
    {
        /// <summary>
        /// Solve time.
        /// </summary>
        public long Time { get; set; }

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
        public DateTime Date { get; set; }

        /// <summary>
        /// Calculated time with flag.
        /// </summary>
        [JsonIgnore]
        public TimeSpan TimeWithFlag => Flag switch
        {
            SolveFlag.OK => TimeAsSpan,
            SolveFlag.Plus2 => TimeAsSpan.Add(TimeSpan.FromSeconds(2)),
            SolveFlag.DNF => TimeSpan.MaxValue,
            _ => throw new ArgumentException(nameof(Flag))
        };
    }
}
