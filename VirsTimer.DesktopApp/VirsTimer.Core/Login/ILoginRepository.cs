using System.Threading.Tasks;
using VirsTimer.Core.Models;
 
namespace VirsTimer.Core.Services.Login
{
    public interface ILoginRepository
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}