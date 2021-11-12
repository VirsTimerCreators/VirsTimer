using System.Threading.Tasks;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Login
{
    /// <summary>
    /// Logs to repository.
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Logs to repository.
        /// </summary>
        Task<RepositoryResponse<IUserClient>> LoginAsync(LoginRequest loginRequest);
    }
}