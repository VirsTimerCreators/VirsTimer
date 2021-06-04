using System.Collections.Generic;

namespace VirsTimer.Core.Services.Cache
{
    public class ApplicationCache : IApplicationCache
    {
        public string LastChoosenEvent { get; set; } = string.Empty;
        public string LastChoosenSession { get; set; } = string.Empty;
        public IDictionary<string, string> EventsNames { get; init; } = new Dictionary<string, string>();
        public IDictionary<string, IDictionary<string, string>> SessionsByEvent { get; init; } = new Dictionary<string, IDictionary<string, string>>();
    }
}
