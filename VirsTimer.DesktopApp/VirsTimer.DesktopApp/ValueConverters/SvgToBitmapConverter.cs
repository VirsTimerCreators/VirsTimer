using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Svg;
using System;
using System.Globalization;
using System.Xml;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class SvgToBitmapConverter : MarkupExtension, IValueConverter
    {
        private static SvgToBitmapConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new SvgToBitmapConverter();
        }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is not string str)
                throw new ArgumentException("Value must be string.", nameof(value));

            var xmldocument = new XmlDocument();
            xmldocument.LoadXml(str);
            var svgDocument = SvgDocument.Open(xmldocument);
            using var bitmap = svgDocument.Draw(1350, 1000);
            var avaloniaBitmap = new Bitmap(bitmap.ToStream());

            return avaloniaBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
