using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Handlers
{
    public interface IHttpResponseHandler
    {
        Task<RepositoryResponse> HandleAsync(Func<Task<HttpResponseMessage>> responseFunc);
        Task<RepositoryResponse<T>> HandleAsync<T>(Func<Task<HttpResponseMessage>> responseFunc);
    }
}