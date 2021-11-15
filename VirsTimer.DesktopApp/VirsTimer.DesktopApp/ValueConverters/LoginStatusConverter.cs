using System;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.DesktopApp.ValueConverters
{
    public class LoginStatusConverter : IExplicitValueConverter<RepositoryResponseStatus, string>
    {
        public string Convert(RepositoryResponseStatus value)
        {
            return value switch
            {
                RepositoryResponseStatus.Ok => string.Empty,
                RepositoryResponseStatus.ClientError => "Nie udało się zalogować. Błędny login lub hasło.",
                RepositoryResponseStatus.NetworkError => "Nie można połączyć się z serwerem.",
                RepositoryResponseStatus.RepositoryError => "Wystąpił błąd po stornie serwera. Nie można się połączyć.",
                RepositoryResponseStatus.UnknownError => "Wystąpił nieznany błąd. Nie można się połączyć.",
                _ => throw new ArgumentException(null, nameof(value))
            };
        }
    }
}