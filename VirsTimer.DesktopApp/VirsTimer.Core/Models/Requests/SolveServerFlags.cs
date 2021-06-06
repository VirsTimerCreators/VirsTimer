using System;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models.Requests
{
    internal static class SolveServerFlags
    {
        public const string Ok = "OK";
        public const string PlusTwo = "PLUS_TWO";
        public const string Dnf = "DNF";

        public static SolveFlag ToSolveFlag(string serverFlag)
        {
            return (serverFlag) switch
            {
                Ok => SolveFlag.OK,
                PlusTwo => SolveFlag.Plus2,
                Dnf => SolveFlag.DNF,
                _ => throw new ArgumentException(null, nameof(serverFlag))
            };
        }

        public static string ConvertFromSolveFlag(SolveFlag flag)
        {
            return (flag) switch
            {
                SolveFlag.OK => Ok,
                SolveFlag.Plus2 => PlusTwo,
                SolveFlag.DNF => Dnf,
                _ => throw new ArgumentException(null, nameof(flag))
            };
        }
    }
}
