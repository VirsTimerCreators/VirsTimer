using System;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    public class Solve
    {
        public TimeSpan Time { get; }
        public SolveFlag Flag { get; set; }
        public DateTime Date { get; }
        public string Scramble { get; }

        public Solve(TimeSpan time, SolveFlag solveFlag, DateTime date, string scramble)
        {
            Time = time;
            Flag = solveFlag;
            Date = DateTime.Now;
            Scramble = scramble;
        }
    }
}
