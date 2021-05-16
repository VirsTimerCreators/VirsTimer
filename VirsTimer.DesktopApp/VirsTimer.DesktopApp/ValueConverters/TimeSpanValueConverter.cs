using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;
using VirsTimer.DesktopApp.Extensions;

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
            return value is not TimeSpan timeSpan
                ? throw new ArgumentException(nameof(timeSpan))
                : timeSpan.ToDynamicString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
