using System.Net;

namespace VirsTimer.Core.Models.Responses
{
    /// <summary>
    /// Class representing repository response.
    /// </summary>
    public class RepositoryResponse
    {
        /// <summary>
        /// Ok repository response.
        /// </summary>
        public static RepositoryResponse Ok { get; } = new RepositoryResponse(RepositoryResponseStatus.Ok, string.Empty);

        /// <summary>
        /// Response status.
        /// </summary>
        public RepositoryResponseStatus Status { get; }

        /// <summary>
        /// Response message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Is response succesfull.
        /// </summary>
        public bool IsSuccesfull => Status == RepositoryResponseStatus.Ok;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse"/> class.
        /// </summary>
        public RepositoryResponse(RepositoryResponseStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse"/> class.
        /// </summary>
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
}