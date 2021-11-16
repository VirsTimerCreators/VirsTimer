using System;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class RegisterStatusConverter : IValueConverter<RepositoryResponse, string>
    {
        public string Convert(RepositoryResponse value)
        {
            var (status, message) = (value.Status, value.Message);
            return (status, message) switch
            {
                (RepositoryResponseStatus.Ok, _) => string.Empty,
                (RepositoryResponseStatus.ClientError, "Username is already taken!") => "Nazwa użytkownika jest już zajęta!",
                (RepositoryResponseStatus.ClientError, "Email is already in use!") => "Adres email jest już zajęty!",
                (RepositoryResponseStatus.ClientError, _) => $"Wystąpił nieznany błąd: {message}",
                (RepositoryResponseStatus.NetworkError, _) => "Nie można połączyć się z serwerem.",
                (RepositoryResponseStatus.RepositoryError, _) => "Wystąpił błąd po stornie serwera.\nNie można się połączyć.",
                (RepositoryResponseStatus.UnknownError, _) => "Wystąpił nieznany błąd.\nNie można się połączyć.",
                _ => throw new ArgumentException(null, nameof(value))
            };
        }
    }
}