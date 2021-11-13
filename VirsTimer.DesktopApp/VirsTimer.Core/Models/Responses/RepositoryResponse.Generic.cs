using System.Net;

namespace VirsTimer.Core.Models.Responses
{
    /// <summary>
    /// Class representing repository response with some value.
    /// </summary>
    public class RepositoryResponse<T> : RepositoryResponse
    {
        /// <summary>
        /// Response value.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse{T}"/> class.
        /// </summary>
        public RepositoryResponse(T value)
            : this(RepositoryResponseStatus.Ok, string.Empty, value)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse{T}"/> class.
        /// </summary>
        public RepositoryResponse(RepositoryResponseStatus status, string message)
            : base(status, message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse{T}"/> class.
        /// </summary>
        public RepositoryResponse(RepositoryResponseStatus status, string message, T value)
            : this(status, message)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse{T}"/> class.
        /// </summary>
        public RepositoryResponse(HttpStatusCode? httpStatusCode, string message)
            : base(httpStatusCode, message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryResponse{T}"/> class.
        /// </summary>
        public RepositoryResponse(RepositoryResponse other, T value)
            : this(other.Status, other.Message, value)
        { }
    }
}