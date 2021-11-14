using System.Threading.Tasks;

namespace VirsTimer.Core.Services.Cache
{
    /// <summary>
    /// Saves <see cref="IApplicationCache"/>.
    /// </summary>
    public interface IApplicationCacheSaver
    {
        /// <summary>
        /// Saves <see cref="IApplicationCache"/>.
        /// </summary>
        public Task SaveCacheAsync(IApplicationCache applicationCache);
    }
}