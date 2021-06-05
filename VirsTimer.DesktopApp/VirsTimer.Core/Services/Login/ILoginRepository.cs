using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;

namespace VirsTimer.Core.Services.Login
{
    public interface ILoginRepository
    {
        Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest);
    }
}