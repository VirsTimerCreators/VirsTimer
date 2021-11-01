using System.Threading.Tasks;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Register
{
    /// <summary>
    /// Singups to the application.
    /// </summary>
    public interface IRegisterRepository
    {
        /// <summary>
        /// Singsup to the application.
        /// </summary>
        public Task<RepositoryResponse> SingupAsync(SingupRequest request);
    }
}
