namespace VirsTimer.Core.Models.Requests
{
    internal class SolvePatchRequest
    {
        public string Solved { get; init; } = string.Empty;

        public SolvePatchRequest(Solve solve)
        {
            Solved = SolveServerFlags.ConvertFromSolveFlag(solve.Flag);
        }
    }
}
