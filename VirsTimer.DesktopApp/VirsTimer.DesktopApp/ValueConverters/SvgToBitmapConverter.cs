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
    public class SvgToBitmapConverter : MarkupExtension, IValueConverter, IValueConverter<string, Bitmap>
    {
        private readonly int _rasterSize = 1500;

        private const string EmptySvg = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 130 98\" width=\"130px\" version=\"1.1\" height=\"98px\"></svg>";
        private static SvgToBitmapConverter? _converter = null;

        public SvgToBitmapConverter() { }

        public SvgToBitmapConverter(int rasterSize)
        {
            _rasterSize = rasterSize;
        }

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

            return Convert(str);
        }

        public Bitmap Convert(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = EmptySvg;

            var xmldocument = new XmlDocument();
            try
            {
                xmldocument.LoadXml(value);
            }
            catch
            {
                xmldocument.LoadXml(EmptySvg);
            }

            var svgDocument = SvgDocument.Open(xmldocument);
            using var bitmap = svgDocument.Draw(_rasterSize, 0);
            var avaloniaBitmap = new Bitmap(bitmap.ToStream());

            return avaloniaBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}