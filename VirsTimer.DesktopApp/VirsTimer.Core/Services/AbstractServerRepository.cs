using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services
{
    public abstract class AbstractServerRepository
    {
        protected IHttpClientFactory HttpClientFactory { get; }
        protected IUserClient UserClient { get; }

        protected AbstractServerRepository(IHttpClientFactory httpClientFactory, IUserClient userClient)
        {
            HttpClientFactory = httpClientFactory;
            UserClient = userClient;
        }

        protected HttpClient CreateHttpClientWithAuth()
        {
            var client = HttpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserClient.Jwt);

            return client;
        }

        protected async Task<RepositoryResponse> CreateRepositoryResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return RepositoryResponse.Ok;

            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new(response.StatusCode, message);
        }

        protected async Task<RepositoryResponse<T>> CreateRepositoryResponseAsync<T>(HttpResponseMessage response)
        {
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var value = JsonSerializer.Deserialize<T>(message, options);
                return new RepositoryResponse<T>(value!);
            }
            return new RepositoryResponse<T>(response.StatusCode, message);
        }

        protected StringContent CreateJsonRequest<T>(T content)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var str = JsonSerializer.Serialize(content, options);
            return new StringContent(str, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}