using System.Threading.Tasks;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Login
{
    public interface ILoginRepository
    {
        Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest);
    }
}