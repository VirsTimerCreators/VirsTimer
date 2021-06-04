namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Solve flag.
    /// </summary>
    public enum SolveFlag
    {
        /// <summary>
        /// Ok - when solve was clean.
        /// </summary>
        OK,

        /// <summary>
        /// +2 - when one side of cube is in wrong position.
        /// </summary>
        Plus2,

        /// <summary>
        /// DNF - when more than one side of cube is in wrong position.
        /// </summary>
        DNF
    }
}
