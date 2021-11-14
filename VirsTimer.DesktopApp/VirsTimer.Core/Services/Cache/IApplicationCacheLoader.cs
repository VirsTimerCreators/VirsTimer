using System.Threading.Tasks;

namespace VirsTimer.Core.Services.Cache
{
    /// <summary>
    /// Loads <see cref="IApplicationCache"/>.
    /// </summary>
    public interface IApplicationCacheLoader
    {
        /// <summary>
        /// Loads <see cref="IApplicationCache"/>.
        /// </summary>
        public Task<IApplicationCache> LoadCacheAsync();
    }
}