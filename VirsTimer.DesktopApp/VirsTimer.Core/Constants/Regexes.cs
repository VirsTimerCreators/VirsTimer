using System.Text.RegularExpressions;

namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Contains general purpuose regexes.
    /// </summary>
    public static class Regexes
    {
        /// <summary>
        /// Regex matching any whitespace.
        /// </summary>
        public static readonly Regex WhiteSpaces = new Regex(@"\s+", RegexOptions.Compiled);
    }
}
