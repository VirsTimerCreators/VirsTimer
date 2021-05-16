using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Globalization;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class DelayStopwatchTimer : MarkupExtension, IValueConverter
    {
        private static DelayStopwatchTimer? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new DelayStopwatchTimer();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Core.Timers.DelayStopwatchTimer delayFireTimer)
                return Brushes.DarkGray;
            else if (delayFireTimer.CountdownStarted && !delayFireTimer.CanFire)
                return Brushes.IndianRed;
            else if (delayFireTimer.CanFire)
                return Brushes.LawnGreen;

            return Brushes.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
