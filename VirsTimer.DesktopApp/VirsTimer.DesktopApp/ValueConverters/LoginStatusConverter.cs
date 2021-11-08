using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class LoginStatusConverter : MarkupExtension, IValueConverter
    {
        private static LoginStatusConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new LoginStatusConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return value is not RepositoryResponseStatus status
                ? throw new ArgumentException(null, nameof(value))
                : status switch
                {
                    RepositoryResponseStatus.Ok => string.Empty,
                    RepositoryResponseStatus.ClientError => "Nie udało się zalogować.\nBłędny login lub hasło.",
                    RepositoryResponseStatus.NetworkError => "Nie można połączyć się z serwerem.",
                    RepositoryResponseStatus.RepositoryError => "Wystąpił błąd po stornie serwera.\nNie można się połączyć.",
                    RepositoryResponseStatus.UnknownError => "Wystąpił nieznany błąd.\nNie można się połączyć.",
                    _ => throw new ArgumentException(null, nameof(value))
                };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
