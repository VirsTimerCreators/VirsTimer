using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;
using VirsTimer.Core.Utils;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class TimeSpanStatisticsValueConverte : MarkupExtension, IValueConverter
    {
        private static TimeSpanStatisticsValueConverte? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new TimeSpanStatisticsValueConverte();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "---";

            return value is not TimeSpan timeSpan
                ? throw new ArgumentException(nameof(timeSpan))
                : (timeSpan == StatisticsUtils.DnfTime
                    ? "DNF"
                    : timeSpan.ToDynamicString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
