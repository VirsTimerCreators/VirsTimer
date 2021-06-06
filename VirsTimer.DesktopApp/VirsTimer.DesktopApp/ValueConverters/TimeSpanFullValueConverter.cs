using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class TimeSpanFullValueConverter : MarkupExtension, IValueConverter
    {
        private static readonly Regex TimeFormat = new(@"([0-9]{2}):([0-5][0-9]:[0-5][0-9]\.[0-9]{2})");

        private static TimeSpanFullValueConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new TimeSpanFullValueConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not TimeSpan timeSpan
                ? throw new ArgumentException(nameof(timeSpan))
                : timeSpan.ToFullString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString() == null)
                return TimeSpan.Zero;

            var match = TimeFormat.Match(value.ToString()!);
            if (!match.Success)
                return TimeSpan.Zero;

            var hours = int.Parse(match.Groups[1].Value);
            var days = hours / 24;
            var daysAndHours = new TimeSpan(days, hours % 24, 0, 0);
            var minutesAndSeconds = TimeSpan.ParseExact(match.Groups[2].Value, "mm\\:ss\\.ff", CultureInfo.InvariantCulture);
            return daysAndHours + minutesAndSeconds;
        }
    }
}
