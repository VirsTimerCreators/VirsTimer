using System.Threading.Tasks;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;

namespace VirsTimer.Core.Services.Register
{
    /// <summary>
    /// Registers to the application.
    /// </summary>
    public interface IRegisterRepository
    {
        /// <summary>
        /// Registers to the application.
        /// </summary>
        public Task<RepositoryResponse> RegisterAsync(RegisterRequest request);
    }
}