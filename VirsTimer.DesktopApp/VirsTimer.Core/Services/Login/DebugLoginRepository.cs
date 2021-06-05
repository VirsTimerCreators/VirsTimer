using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

namespace VirsTimer.Core.Services.Login
{
    public class DebugLoginRepository : ILoginRepository
    {
        public async Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest)
        {
            if (loginRequest.Name == "Stefa" && loginRequest.Password == "123")
            {
                var response = new UserClient()
                {
                    Id = "1",
                    Jwt = "aasjdhaklsjdh"
                };
                return new RepositoryResponse<IUserClient>(response);
            }

            return new RepositoryResponse<IUserClient>(RepositoryResponseStatus.ClientError, string.Empty);
        }
    }
}