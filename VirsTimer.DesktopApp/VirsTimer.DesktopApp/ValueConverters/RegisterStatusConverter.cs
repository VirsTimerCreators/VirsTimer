using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class RegisterStatusConverter : MarkupExtension, IMultiValueConverter
    {
        private static RegisterStatusConverter? _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new RegisterStatusConverter();
        }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count < 2
                || values[0] is not RepositoryResponseStatus status
                || values[1] is not string message)
                return string.Empty;

            return (status, message) switch
            {
                (RepositoryResponseStatus.Ok, _) => string.Empty,
                (RepositoryResponseStatus.ClientError, "Username is already taken!") => "Nazwa użytkownika jest już zajęta!",
                (RepositoryResponseStatus.ClientError, "Email is already in use!") => "Adres email jest już zajęty!",
                (RepositoryResponseStatus.ClientError, _) => $"Wystąpił nieznany błąd: {message}",
                (RepositoryResponseStatus.NetworkError, _) => "Nie można połączyć się z serwerem.",
                (RepositoryResponseStatus.RepositoryError, _) => "Wystąpił błąd po stornie serwera.\nNie można się połączyć.",
                (RepositoryResponseStatus.UnknownError, _) => "Wystąpił nieznany błąd.\nNie można się połączyć.",
                _ => throw new ArgumentException(null, nameof(values))
            };
        }
    }
}