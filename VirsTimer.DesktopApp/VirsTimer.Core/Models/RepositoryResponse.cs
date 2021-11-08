using System.Net;

namespace VirsTimer.Core.Models
{
    public class RepositoryResponse
    {
        public static RepositoryResponse Ok { get; } = new RepositoryResponse(RepositoryResponseStatus.Ok, string.Empty);

        public RepositoryResponseStatus Status { get; }
        public string Message { get; }

        public bool Succesfull => Status == RepositoryResponseStatus.Ok;

        public RepositoryResponse(RepositoryResponseStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public RepositoryResponse(HttpStatusCode? httpStatusCode, string message)
        {
            Status = ConvertHttpStatusCode(httpStatusCode);
            Message = message;
        }

        private static RepositoryResponseStatus ConvertHttpStatusCode(HttpStatusCode? httpStatusCode)
        {
            return httpStatusCode switch
            {
                HttpStatusCode.OK => RepositoryResponseStatus.Ok,
                HttpStatusCode.Created => RepositoryResponseStatus.Ok,
                HttpStatusCode.Unauthorized => RepositoryResponseStatus.ClientError,
                HttpStatusCode.Forbidden => RepositoryResponseStatus.ClientError,
                HttpStatusCode.NotFound => RepositoryResponseStatus.ClientError,
                HttpStatusCode.BadRequest => RepositoryResponseStatus.ClientError,
                HttpStatusCode.UnprocessableEntity => RepositoryResponseStatus.ClientError,
                HttpStatusCode.InternalServerError => RepositoryResponseStatus.RepositoryError,
                HttpStatusCode.ServiceUnavailable => RepositoryResponseStatus.NetworkError,
                _ => RepositoryResponseStatus.UnknownError
            };
        }
    }

    public class RepositoryResponse<T> : RepositoryResponse
    {
        public T Value { get; }

        public RepositoryResponse(T value)
            : base(RepositoryResponseStatus.Ok, string.Empty)
        {
            Value = value;
        }

        public RepositoryResponse(RepositoryResponseStatus status, string message)
            : base(status, message)
        { }

        public RepositoryResponse(RepositoryResponseStatus status, string message, T value)
            : base(status, message)
        {
            Value = value;
        }

        public RepositoryResponse(HttpStatusCode? httpStatusCode, string message)
            : base(httpStatusCode, message)
        { }
    }

    public enum RepositoryResponseStatus
    {
        Ok,
        ClientError,
        RepositoryError,
        NetworkError,
        UnknownError
    }
}
