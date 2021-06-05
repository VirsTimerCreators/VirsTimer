using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

namespace VirsTimer.Core.Services.Login
{
    public class ServerLoginRepository : ILoginRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServerLoginRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest)
        {
            using var client = _httpClientFactory.CreateClient();
            var enpoint = "/api/auth/signin/";
            var httpResponse = await client.PostAsJsonAsync(enpoint, loginRequest).ConfigureAwait(false);
            var message = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (httpResponse.IsSuccessStatusCode)
            {
                var value = JsonSerializer.Deserialize<UserClient>(message);
                return new RepositoryResponse<IUserClient>(value!);
            }
            return new RepositoryResponse<IUserClient>(httpResponse.StatusCode, message);
        }
    }
}

