using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Login
{
    /// <summary>
    /// Virs timer server api <see cref="ILoginRepository"/> implementation. 
    /// </summary>
    public class ServerLoginRepository : ILoginRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLoginRepository"/> class.
        /// </summary>
        public ServerLoginRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var enpoint = Server.Endpoints.Auth.Login;
                var httpResponse = await client.PostAsJsonAsync(enpoint, loginRequest).ConfigureAwait(false);
                var message = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var value = JsonSerializer.Deserialize<UserClient>(message, Json.ServerSerializerOptions);
                    return new RepositoryResponse<IUserClient>(value!);
                }
                return new RepositoryResponse<IUserClient>(httpResponse.StatusCode, message);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return new RepositoryResponse<IUserClient>(RepositoryResponseStatus.NetworkError, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return new RepositoryResponse<IUserClient>(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<IUserClient>(RepositoryResponseStatus.UnknownError, ex.Message);
            }
        }
    }
}