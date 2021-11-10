using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Register
{
    public class ServerRegisterRepository : IRegisterRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpResponseHandler _httpResponseHandler;

        public ServerRegisterRepository(IHttpClientFactory httpClientFactory, IHttpResponseHandler httpResponseHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpResponseHandler = httpResponseHandler;
        }

        public async Task<RepositoryResponse> RegisterAsync(RegisterRequest request)
        {
            using var client = _httpClientFactory.CreateClient();
            var enpoint = "/api/auth/signup/";
            var response = await _httpResponseHandler.HandleAsync(() => client.PostAsJsonAsync(enpoint, request));
            return response;
        }
    }
}
