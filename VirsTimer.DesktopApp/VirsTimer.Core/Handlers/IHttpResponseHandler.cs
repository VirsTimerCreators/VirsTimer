using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Handlers
{
    /// <summary>
    /// Handles getting http responses.
    /// </summary>
    public interface IHttpResponseHandler
    {
        /// <summary>
        /// Calling <paramref name="responseFunc"/> and handles <see cref="HttpResponseMessage"/> wrapping response in <see cref="RepositoryResponse"/>.
        /// </summary>
        Task<RepositoryResponse> HandleAsync(Func<Task<HttpResponseMessage>> responseFunc);

        /// <summary>
        /// Calling <paramref name="responseFunc"/> and handles <see cref="HttpResponseMessage"/> wrapping response in <see cref="RepositoryResponse{T}"/> (repository response with value).
        /// </summary>
        Task<RepositoryResponse<T>> HandleAsync<T>(Func<Task<HttpResponseMessage>> responseFunc);
    }
}