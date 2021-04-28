using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class TimeSpanValueConverter : MarkupExtension, IValueConverter
    {
        private static TimeSpanValueConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new TimeSpanValueConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan timeSpan))
                return string.Empty;
            if (timeSpan.Hours > 0)
                return timeSpan.ToString("hh\\:mm\\:ss\\.ff");
            else if (timeSpan.Minutes > 0)
                return timeSpan.ToString("mm\\:ss\\.ff");
            else
                return timeSpan.ToString("ss\\.ff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
