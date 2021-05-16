using System;

namespace VirsTimer.DesktopApp.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToDynamicString(this TimeSpan ts) => ts switch
        {
            var timeSpan when timeSpan.Hours > 0 => timeSpan.ToString("hh\\:mm\\:ss\\.ff"),
            var timeSpan when timeSpan.Minutes > 0 => timeSpan.ToString("mm\\:ss\\.ff"),
            _ => ts.ToString("ss\\.ff")
        };
    }
}
