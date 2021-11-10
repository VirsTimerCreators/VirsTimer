using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class AlertFromBoolConverter : MarkupExtension, IMultiValueConverter
    {
        private static AlertFromBoolConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new AlertFromBoolConverter();
        }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count < 2
                || values[0] is not bool boolParam
                || values[1] is not string stringParam)
                return false;

            return !string.IsNullOrEmpty(stringParam) && !boolParam;
        }
    }
}