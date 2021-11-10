using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Handlers
{
    public class HttpResponseHandler : IHttpResponseHandler
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<RepositoryResponse> HandleAsync(Func<Task<HttpResponseMessage>> responseFunc)
        {
            try
            {
                var httpResponse = await responseFunc().ConfigureAwait(false);
                var message = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                    return RepositoryResponse.Ok;

                var error = JsonSerializer.Deserialize<ErrorResponse>(message, JsonSerializerOptions);
                return new RepositoryResponse(httpResponse.StatusCode, error?.Message ?? message);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return new RepositoryResponse(RepositoryResponseStatus.NetworkError, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }

        public async Task<RepositoryResponse<T>> HandleAsync<T>(Func<Task<HttpResponseMessage>> responseFunc)
        {
            try
            {
                var httpResponse = await responseFunc().ConfigureAwait(false);
                var message = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var value = JsonSerializer.Deserialize<T>(message);
                    return new RepositoryResponse<T>(value!);
                }
                return new RepositoryResponse<T>(httpResponse.StatusCode, message);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return new RepositoryResponse<T>(RepositoryResponseStatus.NetworkError, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<T>(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<T>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }
    }
}