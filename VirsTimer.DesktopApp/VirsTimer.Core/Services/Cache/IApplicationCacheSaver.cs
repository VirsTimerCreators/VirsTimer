using System.Threading.Tasks;

namespace VirsTimer.Core.Services.Cache
{
    public interface IApplicationCacheSaver
    {
        IApplicationCache ApplicationCache { get; }
        public Task UpdateCacheAsync();
    }
}
