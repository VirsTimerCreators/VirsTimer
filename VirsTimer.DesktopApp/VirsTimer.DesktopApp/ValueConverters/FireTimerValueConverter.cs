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
            if (!(value is int delayFireTimer))
                return Brushes.Yellow;
            if (delayFireTimer == 0)
                return Brushes.Khaki;
            if (delayFireTimer == 1)
                return Brushes.MediumVioletRed;

            return Brushes.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
