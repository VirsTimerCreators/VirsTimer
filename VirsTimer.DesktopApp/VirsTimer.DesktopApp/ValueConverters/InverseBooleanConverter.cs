using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;

namespace VirsTimer.DesktopApp.ValueConverters
{
    class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        private static InverseBooleanConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new InverseBooleanConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not bool boolean
                ? throw new ArgumentException("Value must be bool", nameof(value))
                : !boolean;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
