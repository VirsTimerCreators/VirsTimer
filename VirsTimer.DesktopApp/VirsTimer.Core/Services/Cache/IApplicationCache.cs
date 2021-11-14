using System.Collections.Generic;

namespace VirsTimer.Core.Services.Cache
{
    /// <summary>
    /// Application cache.
    /// </summary>
    public interface IApplicationCache
    {
        /// <summary>
        /// Event name by its id.
        /// </summary>
        IDictionary<string, string> EventsById { get; }

        /// <summary>
        /// Sessions dictionary (sessionId, sessionName) by event id to which they belong.
        /// </summary>
        IDictionary<string, IDictionary<string, string>> SessionsByEventId { get; }

        /// <summary>
        /// Last choosen event after app close.
        /// </summary>
        string LastChoosenEvent { get; set; }

        /// <summary>
        /// Last choosen session after app close.
        /// </summary>
        string LastChoosenSession { get; set; }
    }
}