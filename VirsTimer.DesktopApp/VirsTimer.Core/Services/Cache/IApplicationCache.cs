using System.Collections.Generic;

namespace VirsTimer.Core.Services.Cache
{
    public interface IApplicationCache
    {
        IDictionary<string, string> EventsNames { get; init; }
        IDictionary<string, IDictionary<string, string>> SessionsByEvent { get; init; }
        string LastChoosenEvent { get; set; }
        string LastChoosenSession { get; set; }
    }
}
