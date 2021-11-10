using System.Text.RegularExpressions;
 
namespace VirsTimer.DesktopApp.Constants
{
    public static class Events
    {
        public const string NewEventNameBase = "Event";
 
        public static readonly Regex NewEventNameRegex = new($"{NewEventNameBase}([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
