using System;

namespace VirsTimer.DesktopApp.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToDynamicString(this TimeSpan ts) => ts switch
        {
            var timeSpan when timeSpan.Hours > 0 => $"{(int)timeSpan.TotalHours}:" + timeSpan.ToString("mm\\:ss\\.ff"),
            var timeSpan when timeSpan.Minutes > 0 => timeSpan.ToString("mm\\:ss\\.ff"),
            _ => ts.ToString("ss\\.ff")
        };

        public static string ToFullString(this TimeSpan ts)
        {
            return $"{(int)ts.TotalHours:00}:" + ts.ToString("mm\\:ss\\.ff");
        }
    }
}
