using System.Text.RegularExpressions;

namespace VirsTimer.DesktopApp.Constants
{
    public static class Sessions
    {
        public const string NewSessionNameBase = "Sesja";

        public static readonly Regex NewSessionNameRegex = new($"{NewSessionNameBase}([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
