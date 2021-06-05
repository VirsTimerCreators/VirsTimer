using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

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


        protected string CreateEndpoint(params string[] values)
        {
            var endpointBase = new[] { Server.Endpoints.Users, UserClient.Id };
            var endpointValues = endpointBase.Concat(values).ToArray();
            var endpoint = Path.Combine(endpointValues);
            return endpoint;
        }
    }
}
