using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using VirsTimer.Core.Timers;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class FireTimerValueConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //    if (!(value is DelayFireTimer delayFireTimer))
        //        return Brushes.DarkGray;
        //    if (delayFireTimer.CanFire)
        //        return Brushes.GreenYellow;
        //    if (!delayFireTimer.CanFire && delayFireTimer.IsRunning)
        //        return Brushes.MediumVioletRed;

        //    return Brushes.DarkGray;
        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DelayStopwatchTimer delayFireTimer))
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
