using System;
using System.Text.Json.Serialization;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    public class Solve
    {
        public Guid Id { get; }
        public long Time { get; }

        [JsonIgnore]
        public TimeSpan TimeAsSpan => TimeSpan.FromTicks(Time);
        public SolveFlag Flag { get; set; }
        public DateTime Date { get; }
        public string Scramble { get; }

        [JsonConstructor]
        public Solve(Guid id, long time, SolveFlag flag, DateTime date, string scramble)
        {
            Id = id;
            Time = time;
            Flag = flag;
            Date = date;
            Scramble = scramble;
        }

        public Solve(TimeSpan time, string scramble)
        {
            Id = Guid.NewGuid();
            Time = time.Ticks;
            Flag = SolveFlag.OK;
            Date = DateTime.Now;
            Scramble = scramble;
        }
    }
}
