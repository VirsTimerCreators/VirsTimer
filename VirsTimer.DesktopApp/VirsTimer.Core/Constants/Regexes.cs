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
        public static readonly Regex WhiteSpaces = new(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// Regex matching files with json extension.
        /// </summary>
        public static readonly Regex JsonFile = new("([^<>:\"/\\|?*]+)\\.json", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
