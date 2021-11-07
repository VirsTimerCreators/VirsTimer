using System.Threading.Tasks;
using VirsTimer.Core.Models;
 
namespace VirsTimer.Core.Services.Login
{
    public class DebugLoginRepository : ILoginRepository
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            if (loginRequest.Name == "Stefa" && loginRequest.Password == "123")
            {
                var response = new LoginResponse()
                {
                    UserId = "1",
                    Succesfull = true,
                    Jwt = "aasjdhaklsjdh"
                };
                return response;
            }
 
 
            var badResponse = new LoginResponse() { Succesfull = false };
            return badResponse;
        }
    }
}
