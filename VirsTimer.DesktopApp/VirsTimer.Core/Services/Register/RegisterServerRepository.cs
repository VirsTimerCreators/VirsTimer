using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Register
{
    /// <summary>
    /// Virs timer server api <see cref="IRegisterRepository"/> implementation. 
    /// </summary>
    public class RegisterServerRepository : IRegisterRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="IRegisterRepository"/> class.
        /// </summary>
        public RegisterServerRepository(
            IHttpClientFactory httpClientFactory,
            IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<RepositoryResponse> RegisterAsync(RegisterRequest request)
        {
            using var client = _httpClientFactory.CreateClient();
            var enpoint = Server.Endpoints.Auth.Register;
            var response = await _httpResponseHandler.HandleAsync(() => client.PostAsJsonAsync(enpoint, request));
            return response;
        }
    }
}