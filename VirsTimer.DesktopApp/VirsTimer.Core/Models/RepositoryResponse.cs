using System.Net;

namespace VirsTimer.Core.Models
{
    public class RepositoryResponse
    {
        public static RepositoryResponse Ok { get; } = new RepositoryResponse(RepositoryResponseStatus.Ok, string.Empty);

        public RepositoryResponseStatus Status { get; }
        public string Message { get; }

        public RepositoryResponse(RepositoryResponseStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public RepositoryResponse(HttpStatusCode httpStatusCode, string message)
        {
            Status = ConvertHttpStatusCode(httpStatusCode);
            Message = message;
        }

        private static RepositoryResponseStatus ConvertHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode switch
            {
                HttpStatusCode.OK | HttpStatusCode.Created => RepositoryResponseStatus.Ok,
                HttpStatusCode.Unauthorized | HttpStatusCode.Forbidden | HttpStatusCode.NotFound | HttpStatusCode.BadRequest | HttpStatusCode.UnprocessableEntity => RepositoryResponseStatus.ClientError,
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

        public RepositoryResponse(HttpStatusCode httpStatusCode, string message, T value)
            : base(httpStatusCode, message)
        {
            Value = value;
        }
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
